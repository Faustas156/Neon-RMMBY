using RMMBY.Editable;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;
using MelonLoader;
using RMMBY.Helpers;

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