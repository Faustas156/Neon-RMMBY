using RMMBY.Editable;
using RMMBY.Helpers;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace RMMBY
{
    internal class InfoMenuHandler : MonoBehaviour
    {
        public string sceneToLoad = "";

        public bool setup;

        private void SetResolution()
        {
            float referenceResolution = float.Parse(DataReader.ReadData("ModMenuScreenResolutionReference"));
            Canvas modmenu = GameObject.Find("ModMenu").GetComponent<Canvas>();
            float screenHeight = Screen.height;
            float scaleFactor = screenHeight / referenceResolution;
            modmenu.scaleFactor = scaleFactor;
        }
    }
}
