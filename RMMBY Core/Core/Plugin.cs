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
        private EnabledMods em;
        private InputHandler inputHandler;

        private string infoText = "";
        private int infoType;

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

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            // em stands for "enabled mods"
            //this is a pretty important if statement, without it, entire screen ends up like that one neonnetwork problem lol
            if (em == null) 
            {
                em = new GameObject().AddComponent<EnabledMods>();
                em.name = "RMMBY";
                em.gameObject.AddComponent<InputHandler>();
                inputHandler = em.gameObject.GetComponent<InputHandler>();
            }

            if (sceneName == "RMMBYModMenu")
            {
                GameObject go = new GameObject();
                go.AddComponent<ModMenuHandler>();

                inputHandler.OnSceneLoaded();
                inputHandler.active = true;
            } 

            else
            {
                inputHandler.active = false;
            }

            if (sceneName == "Menu")
            {
                if (!ListenToLoadMenu.setMenuFunction) return;

                inMenu = true;
            }
        }
    }
}
