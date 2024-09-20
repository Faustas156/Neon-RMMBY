using MelonLoader;
using RMMBY.Helpers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RMMBY
{
    public class EnabledMods : MonoBehaviour
    {
        private string path;
        public List<string> enabledPaths = new List<string>();

        private void Start()
        {
            DontDestroyOnLoad(this);
            GetPath();
        }

        public void GetPath()
        {
            path = Path.Combine(MelonHandler.ModsDirectory, "RMMBY\\data");

            string datapath = DataReader.ReadData("datapath");

            if (datapath == "INVALID DATA TYPE")
            {
                List<string> lines = new List<string>();
                lines.Add(string.Concat("datapath;", path));

                WriteToFile.WriteFile(path, lines.ToArray(), true);
            }
            else if (datapath != path)
            {
                WriteToFile.ReplaceLine(path, datapath, path, 1, false);
                RemoveAllEnabledPaths();
            }
        }

        internal void RemoveAllEnabledPaths()
        {
            string[] data = DataReader.ReadAllData();

            List<string> lines = new List<string>();

            for (int i = 0; i < data.Length; i++)
            {
                WriteToFile.ReplaceLine(path, "enabledmod", "", 0, true);
            }
        }
    }
}