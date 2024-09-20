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

        public static event Action onSettingsChanged; //this singlehandedly carries the whole button appearing stuff, DON'T DELETE IT

        private GameObject titlePanel;

        public void Start()
        {
            buttonPrefab = GameObject.Find("ButtonPrefab");
            buttonHolder = GameObject.Find("Buttons");
            GetMenus();
            ToggleMenu(0);
            currentMenu = 0;

            modmenu = GameObject.Find("ModMenu").GetComponent<Canvas>();
            float screenHeight = Screen.height;
            modmenu.scaleFactor = scaleFactor;

            titlePanel = GameObject.FindObjectOfType<MainMenu>().transform.Find("Canvas").Find("Main Menu").Find("Panel").Find("Title Panel").gameObject;
            titlePanel.SetActive(false);
        }

        private void GetMenus()
        {
            menus.Clear();
            menus.Add(GameObject.Find("ModSelectionMenu"));
        }

        private void ToggleMenu(int menuID)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if (i != menuID)
                {
                    menus[i].SetActive(false);
                }
                else
                {
                    menus[i].SetActive(true);
                }
            }
        }

        private float currentY;
        private float currentX;

        private GameObject buttonHolder;
        private List<GameObject> buttons = new List<GameObject>();
        public GameObject selectedObject;

        public GameObject buttonPrefab;
        public GameObject settingPrefab;
        private int currentMenu = -1;

        public static List<MetadataBase> Metadata = new List<MetadataBase>();
        public Text[] modText;


        private List<GameObject> menus = new List<GameObject>();
    }
}