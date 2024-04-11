using MelonLoader;
using UnityEngine;
using RMMBY.Editable;
using RMMBY.Helpers;
using System;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

namespace RMMBY
{
    public class Plugin : MelonMod
    {
        private bool inScene;
        private EnabledMods em;
        private InputHandler inputHandler;

        private bool holdConsoleToggle;
        private bool consoleHidden;

        private string infoText = "";
        private int infoType;
        private bool inInfo;

        private bool inMenu;

        public void LoadInfo(string message, int type)
        {
            infoText = message;
            infoType = type;

            LoadModMenu.CheckForBundle("RMMBYInfo");
        }

        public void LoadInfo(int type)
        {
            switch (type)
            {
                case 0:
                    LoadModMenu.CheckForBundle("RMMBYInfo");
                    break;
            }
        }

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();

            //ShowWindow(GetConsoleWindow(), 0);
            //consoleHidden = true;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            // em stands for "enabled mods"
            if (em == null)
            {
                em = new GameObject().AddComponent<EnabledMods>();
                em.name = "RMMBY";
                em.gameObject.AddComponent<InputHandler>();
                inputHandler = em.gameObject.GetComponent<InputHandler>();
            }

            if (sceneName == ListenToLoadMenu.sceneToListen)
            {
                inScene = true;
                ListenToLoadMenu.OnSceneStart();
            }
            else
            {
                inScene = false;
            }

            if (sceneName == "RMMBYModMenu")
            {
                GameObject go = new GameObject();
                go.AddComponent<ModMenuHandler>();

                inputHandler.OnSceneLoaded();
                inputHandler.active = true;
            } else if (sceneName == "RMMBYInfo")
            {
                GameObject go = new GameObject();
                go.AddComponent<InfoMenuHandler>();

                inputHandler.OnSceneLoaded();
                inputHandler.active = true;

                inInfo = true;
            }
            else
            {
                inputHandler.active = false;
            }

            if(sceneName == "Menu")
            {
                if (!ListenToLoadMenu.setMenuFunction) return;

                ListenToLoadMenu.UpdateForMods();

                inMenu = true;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (inInfo) SetInfo();

            if (inScene && ListenToLoadMenu.runOnUpdate)
            {
                ListenToLoadMenu.OnSceneUpdate();
            }
        }

        public void SetInfo()
        {
            if (!GameObject.FindObjectOfType<InfoMenuHandler>()) return;

            switch (infoType)
            {
                case 0:
                    GameObject.FindObjectOfType<InfoMenuHandler>().SetupRestart(infoText);
                    break;
            }

            inInfo = false;
        }
    }
}
