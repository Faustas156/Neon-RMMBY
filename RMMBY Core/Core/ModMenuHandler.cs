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
            CreateButtons();
            GetMenus();
            ToggleMenu(0);

            modmenu = GameObject.Find("ModMenu").GetComponent<Canvas>();
            float screenHeight = Screen.height;
            modmenu.scaleFactor = scaleFactor;

            titlePanel = GameObject.FindObjectOfType<MainMenu>().transform.Find("Canvas").Find("Main Menu").Find("Panel").Find("Title Panel").gameObject;
            titlePanel.SetActive(false);
        }


        private void CreateButtons()
        {
            for (int i = 0; i < Metadata.Count; i++)
            {
                GameObject button = GameObject.Instantiate(buttonPrefab);
                button.transform.SetParent(buttonHolder.transform);
                button.transform.SetSiblingIndex(i);
                button.transform.localPosition = new Vector3(currentX, currentY, 0);
                if (i != 0)
                {
                    button.transform.Find("Highlight").gameObject.SetActive(false);
                }

                button.transform.Find("Text (Legacy)").GetComponent<Text>().text = Metadata[i].Title;

                if (Metadata[i].Title.EndsWith("(Update)"))
                {
                    button.transform.Find("Text (Legacy)").GetComponent<Text>().color = Color.green;
                }

                button.AddComponent<ModToggleButton>();

                buttons.Add(button);
            }
        }

        private void GetMenus()
        {
            menus.Clear();
            menus.Add(GameObject.Find("ModSelectionMenu"));
            menus.Add(GameObject.Find("ModSettingsMenu"));
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

        public static List<MetadataBase> Metadata = new List<MetadataBase>();
        public Text[] modText;


        private List<GameObject> menus = new List<GameObject>();
    }
}