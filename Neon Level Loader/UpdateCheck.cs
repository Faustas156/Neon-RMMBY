using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RMMBY.NeonLevelLoader
{
    internal class UpdateCheck
    {
        public static void UpdateForLevels()
        {

            GameObject button = GameObject.Find("CL Button");

            ColorBlock cb = button.transform.Find("Button").GetComponent<Button>().colors;
            
            cb.normalColor = Color.white;
            button.transform.Find("Button").Find("Text").GetComponent<TMP_Text>().text = "Custom Levels";
            button.transform.Find("Button").GetComponent<Button>().colors = cb;

            button.transform.Find("Button").GetComponent<Button>().OnPointerExit(null);

            GameObject.FindObjectOfType<MenuScreenTitle>().LoadButtons();
        }
    }
}
