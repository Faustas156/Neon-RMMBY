﻿using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace RMMBY.NeonLevelLoader
{
    public class Plugin : MelonMod
    {
        private bool inMenu;
        private bool buttonExists;

        public AssetBundle bundle;
        public MetadataLevel currentLevel;

        public int insight;

        public GameObject[] resultsButtons;

        public GameObject[] pauseButtons;

        public bool addedButton;

        public GameObject resultsLeader;
        public GameObject stagingLeader;
        public GameObject levelName;
        public GameObject levelTitle;
        public GameObject levelEnvironment;

        public bool waitForTitle;

        public bool customLevelButtonDelegateSet;

        private int uploadShouldBeDisabled = 0;

        const long collectAfterAllocating = 16 * 1024 * 1024;

        private bool didSetup;

        private float garbageTimer = 30;

        public string LevelID()
        {
            string result = "";

            if (currentLevel != null)
            {
                result = string.Concat(currentLevel.Author, currentLevel.Title, currentLevel.Version).Replace(" ", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("*", "").Replace("\"", "").Replace("<", "").Replace(">", "").Replace("|", "");
            }

            return result;
        }

        public override void OnLateInitializeMelon()
        {
            base.OnLateInitializeMelon();

            GarbageCollector.GCMode = GarbageCollector.Mode.Manual; 
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GCSettings.LatencyMode = GCLatencyMode.Interactive;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            if (sceneName == "CustomLevel")
            {

                LevelSetup.Setup();
                LoggerInstance.Msg("Setting Up Level");
                ToggleLeaderboardUpload(true);
                SetCustomLevelButtons();
            }
            else if (resultsButtons != null && resultsButtons.Length != 0)
            {
                    resultsButtons[0].SetActive(true);
                    resultsButtons[1].SetActive(true);
                    resultsButtons[2].SetActive(false);

                    pauseButtons[0].SetActive(true);
                    pauseButtons[1].SetActive(true);
                    pauseButtons[2].SetActive(false);
                    pauseButtons[3].SetActive(false);
            }

            if (sceneName == "CustomLevelMenu")
            {
                LoggerInstance.Msg("CLM Loaded");

                UnityEngine.Object.Instantiate(new GameObject()).AddComponent<MenuHandler>();
                //GameObject manager = GameObject.Instantiate(new GameObject());
                //manager.AddComponent<MenuHandler>();

                CreateLevel.AddLevelCustomCampaign();

                ToggleLeaderboardUpload(false);

                inMenu = false;
            }
            else if (sceneName == "Menu")
            {
                inMenu = true;
                waitForTitle = false;

                MenuFunction();
            }
            else inMenu = false;

            long mem = Profiler.GetMonoHeapSizeLong();
            if (mem > collectAfterAllocating)
            {
                GC.Collect(0);
                garbageTimer = 30;
            }
        }
        private void MenuFunction()
        {
            if (uploadShouldBeDisabled == 0)
            {
                switch (GameDataManager.powerPrefs.dontUploadToLeaderboard)
                {
                    case true:
                        uploadShouldBeDisabled = 2;
                        break;
                    case false:
                        uploadShouldBeDisabled = 1;
                        break;
                }
            }

            if (resultsButtons == null)
            {
                ButtonGenerators.CreateResults();
            }
            if (pauseButtons == null)
            {
                ButtonGenerators.CreatePause();
            }

            ToggleLeaderboardUpload(false); //this could also be mega faulty lol
        }

        private void GetLeaderboard()
        {
            if (resultsLeader == null)
            {
                try
                {
                    resultsLeader = GameObject.Find("Ingame Menu").transform.Find("Menu Holder").Find("Results Panel").Find("Leaderboards And LevelInfo").Find("Leaderboards").gameObject;
                }
                catch { }
            }

            if (stagingLeader == null) //fix this whenever possible, very faulty patching but it is what it is
            {
                try
                {
                    stagingLeader = GameObject.Find("Ingame Menu").transform.Find("Menu Holder").Find("Staging Panel").Find("Leaderboards And LevelInfo").Find("Leaderboards").gameObject;
                    levelTitle = stagingLeader.transform.parent.parent.Find("Level Title").gameObject;
                    levelName = stagingLeader.transform.parent.Find("Level Panel/Info Holder/Stats/Level Name").gameObject;
                    levelEnvironment = levelTitle.transform.parent.Find("Level Environment").gameObject;
                }
                catch { }
            }
        }

        private void ToggleLeaderboardUpload(bool turnOn) 
        {
            if (uploadShouldBeDisabled == 1)
            {
                if (turnOn) GameDataManager.powerPrefs.dontUploadToLeaderboard = true;
                else GameDataManager.powerPrefs.dontUploadToLeaderboard = false;
            }
            else if (uploadShouldBeDisabled == 2) GameDataManager.powerPrefs.dontUploadToLeaderboard = true;
        }

        public void SetCustomLevelButtons()
        {
            resultsButtons[0].SetActive(false);
            resultsButtons[1].SetActive(false);
            resultsButtons[2].SetActive(true);

            pauseButtons[0].SetActive(false);
            pauseButtons[1].SetActive(false);
            pauseButtons[2].SetActive(true);
            pauseButtons[3].SetActive(true);

            if (!customLevelButtonDelegateSet)
            {
                pauseButtons[3].GetComponent<MenuButtonHolder>().onClickEvent.RemoveAllListeners();
                pauseButtons[3].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                pauseButtons[3].GetComponent<MenuButtonHolder>().onClickEvent.AddListener(delegate { ReloadLevel(); });
                pauseButtons[3].transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { ReloadLevel(); });

                customLevelButtonDelegateSet = true;
            }
        }

        public override void OnUpdate()
        {
            if (inMenu)
            {
                CreateMenuButton();
                GetLeaderboard();
                didSetup = true;
            }

            if (!didSetup) return;

            if (resultsButtons == null)
            {
                ButtonGenerators.CreateResults();
            }
            if (pauseButtons == null)
            {
                ButtonGenerators.CreatePause();
            }

            //if (pauseButtons[1].activeSelf || resultsButtons[1].activeSelf) SetCustomLevelButtons();

            if (Singleton<Game>.Instance.GetCurrentLevel().levelID == LevelID() && (pauseButtons[1].activeSelf || resultsButtons[1].activeSelf)) SetCustomLevelButtons();
        }

        public GameObject IndividualMenuButton(string text, string objName)
        {
            GameObject result = GameObject.Instantiate(GameObject.Find("Quit Button"));

            result.transform.SetParent(GameObject.Find("Title Buttons").transform);

            result.transform.Find("Button").Find("Text").GetComponent<TMP_Text>().text = text;

            result.name = objName;
            result.transform.localScale = Vector3.one;

            return result;
        }

        public void CreateMenuButton()
        {
            if (GameObject.Find("CL Button") || buttonExists)
            {
                if (buttonExists)
                {
                    inMenu = false;
                    UnityEngine.Object.FindObjectOfType<MenuScreenTitle>().LoadButtons();
                }
                return;
            }
            if (!GameObject.Find("Quit Button")) return;

            GameObject clbutton = IndividualMenuButton("Custom Levels", "CL Button");
            clbutton.GetComponent<MenuButtonHolder>().ForceVisible();

            clbutton.GetComponent<MenuButtonHolder>().onClickEvent.RemoveAllListeners();
            clbutton.transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
            clbutton.GetComponent<MenuButtonHolder>().onClickEvent.AddListener(delegate { Melon<Plugin>.Instance.LoadMenu(); });
            clbutton.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { Melon<Plugin>.Instance.LoadMenu(); });

            clbutton.transform.SetSiblingIndex(3);

            MenuScreenTitle ms = GameObject.FindObjectOfType<MenuScreenTitle>();
            ms.buttonsToLoad.Add(clbutton.GetComponent<MenuButtonHolder>());

            List<MenuButtonHolder> list = new List<MenuButtonHolder>();

            for (int i = 0; i < 3; i++)
            {
                list.Add(ms.buttonsToLoad[i]);
            }

            list.Add(ms.buttonsToLoad[5]);
            list.Add(ms.buttonsToLoad[3]);
            list.Add(ms.buttonsToLoad[6]);
            list.Add(ms.buttonsToLoad[4]);

            ms.buttonsToLoad = list;

            ms.LoadButtons();

            buttonExists = true;
            inMenu = false;

            //if (!GameObject.Find("CL Button") && !buttonExists)
            //{
            //if (!GameObject.Find("Quit Button")) return;
            }

        public void LoadMenu() => LoadModMenu.CheckForBundle("CustomLevelMenu");

        public void ReloadLevel()
        {
            if (SceneManager.GetActiveScene().name == "CustomLevel")
            {
                bundle.Unload(true);

                string path = currentLevel.Location;
                bundle = AssetBundle.LoadFromFile(Path.Combine(path, currentLevel.AssetBundleName));
                Singleton<Game>.Instance.PlayLevel(LevelID(), true, null);
            }
        }

        [HarmonyPatch(typeof(Leaderboards), "SetLevel", new Type[] { typeof(LevelData), typeof(bool), typeof(bool) })]
        private static class LeaderboardPatch
        {
            private static bool Prefix()
            {
                if (Singleton<Game>.Instance.GetCurrentLevel().levelID == Melon<Plugin>.Instance.LevelID())
                {
                    Melon<Plugin>.Instance.resultsLeader.SetActive(false);
                    Melon<Plugin>.Instance.stagingLeader.SetActive(false);
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(MenuButtonHolder), "OnEnable", null)]
        private static class ResultsPatch
        {
            private static void Prefix()
            {
                if (SceneManager.GetActiveScene().name == "CustomLevel")
                {
                    Melon<Plugin>.Instance.resultsButtons[0].SetActive(false);
                    Melon<Plugin>.Instance.resultsButtons[1].SetActive(false);
                    Melon<Plugin>.Instance.resultsButtons[2].SetActive(true);
                }
            }
        }

        [HarmonyPatch(typeof(MenuScreenPause), "OnSetVisible", null)]
        private static class PausePatch
        {
            private static void Postfix()
            {
                if (SceneManager.GetActiveScene().name == "CustomLevel")
                {
                    Melon<Plugin>.Instance.pauseButtons[0].SetActive(false);
                    Melon<Plugin>.Instance.pauseButtons[1].SetActive(false);
                    Melon<Plugin>.Instance.pauseButtons[2].SetActive(true);
                    Melon<Plugin>.Instance.pauseButtons[3].SetActive(true);

                    GameObject.FindObjectOfType<MenuScreenPause>().LoadButtons();
                }
            }
        }

        [HarmonyPatch(typeof(MenuScreenStaging), "OnSetVisible", null)]
        private static class LevelTextPatch
        {
            private static void Postfix()
            {
                if (SceneManager.GetActiveScene().name == "CustomLevel")
                {
                    Melon<Plugin>.Instance.levelName.GetComponent<TMP_Text>().text = Melon<Plugin>.Instance.currentLevel.Title;
                    Melon<Plugin>.Instance.levelTitle.GetComponent<TMP_Text>().text = Melon<Plugin>.Instance.currentLevel.Title;
                    Melon<Plugin>.Instance.levelEnvironment.GetComponent<TMP_Text>().text = "DISTRICT: " + LevelSetup.GetDistrictName(Melon<Plugin>.Instance.currentLevel.EnvironmentType);
                }
            }
        }
    }
}
