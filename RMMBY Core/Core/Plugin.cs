using MelonLoader;
using RMMBY.Editable;
using UnityEngine;

namespace RMMBY
{
    public class Plugin : MelonMod
    {
        private EnabledMods em;
        private InputHandler inputHandler;

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

            else
            {
                inputHandler.active = false;
            }
        }
    }
}
