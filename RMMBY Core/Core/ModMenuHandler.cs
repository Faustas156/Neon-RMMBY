using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RMMBY
{
    public class ModMenuHandler : MonoBehaviour
    {

        Canvas modmenu;
        private float scaleFactor = 1;

        public void Start()
        {
            buttonPrefab = GameObject.Find("ButtonPrefab");
            buttonHolder = GameObject.Find("Buttons");
            GetMenus();

            modmenu = GameObject.Find("ModMenu").GetComponent<Canvas>();
            float screenHeight = Screen.height;
            modmenu.scaleFactor = scaleFactor;
        }

        private void GetMenus()
        {
            menus.Clear();
            menus.Add(GameObject.Find("ModSelectionMenu"));
        }

        private GameObject buttonHolder;
        public GameObject selectedObject;

        public GameObject buttonPrefab;
        public GameObject settingPrefab;

        public static List<MetadataBase> Metadata = new List<MetadataBase>();
        public Text[] modText;


        private List<GameObject> menus = new List<GameObject>();
    }
}