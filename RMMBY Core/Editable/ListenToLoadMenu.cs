using HarmonyLib;
using UnityEngine;
using System;

using TMPro;
using UnityEngine.UI;
using System.Reflection;
using System.Collections.Generic;
using MelonLoader;

namespace RMMBY.Editable
{
    public class ListenToLoadMenu
    {
        public static string sceneToListen = "Menu";
        public static bool runOnUpdate = true;
        public static string sceneToReturnTo = "Menu";

        public static bool setMenuFunction = false;

        public static void OnSceneStart()
        {
        }

        public static void OnSceneUpdate()
        {
        }

        public static void LoadMenu()
        {
            LoadModMenu.CheckForBundle("RMMBYModMenu");
        }

        public static void UpdateForMods()
        {
        }
    }
}