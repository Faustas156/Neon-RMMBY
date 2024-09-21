using MelonLoader;
using RMMBY.Helpers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RMMBY
{
    public class EnabledMods : MonoBehaviour
    {
        public List<string> enabledPaths = new List<string>();

        private void Start()
        {
            DontDestroyOnLoad(this);
        }
    }
}