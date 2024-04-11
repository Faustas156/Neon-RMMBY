using MelonLoader;
using RMMBY.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            GetEnabledPaths();
        }

        public void GetPath()
        {
            path = Path.Combine(MelonHandler.ModsDirectory, "RMMBY\\data");

            string datapath = DataReader.ReadData("datapath");

            if(datapath == "INVALID DATA TYPE")
            {
                List<string> lines = new List<string>();
                lines.Add(string.Concat("datapath;", path));

                WriteToFile.WriteFile(path, lines.ToArray(), true);
            } else if(datapath != path)
            {
                WriteToFile.ReplaceLine(path, datapath, path, 1, false);
                RemoveAllEnabledPaths();
            }
        }

        public void GetEnabledPaths()
        {
            enabledPaths.Clear();

            enabledPaths = DataReader.ReadDataAll("enabledmod").ToList();
        }

        internal void AddNewEnabledPath(string newpath)
        {
            if (CheckEnabled(newpath)) return;

            for (int i = 0; i < enabledPaths.Count; i++)
            {
                if (enabledPaths[i] == newpath) return;
            }

            WriteToFile.WriteFile(path, new string[1] { "enabledmod;" + newpath }, true);

            GetEnabledPaths();

        }

        internal void RemoveEnabledPath(string newpath)
        {
            if (!CheckEnabled(newpath)) return;

            WriteToFile.ReplaceLine(path, newpath, "", 1, true);

            GetEnabledPaths();

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

        public bool CheckEnabled(string newpath)
        {
            for (int i = 0; i < enabledPaths.Count; i++)
            {
                if (enabledPaths[i] == newpath)
                {
                    return true;
                }
            }

            return false;
        }
    }
}