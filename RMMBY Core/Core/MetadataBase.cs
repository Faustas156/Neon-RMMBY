using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace RMMBY
{
    public class MetadataBase
    {
        public string Title { get; set; } = "N/A";
        public string Description { get; set; } = "N/A";
        public string Version { get; set; } = "N/A";
        public string Author { get; set; } = "N/A";
        public string Location { get; private set; }
        public MetadataState State { get; private set; } = MetadataState.Success;

        public static T Load<T>(string path) where T : MetadataBase
        {
            T t;
            try
            {
                t = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                t.Location = Path.GetDirectoryName(path);
            }
            catch (JsonReaderException)
            {
                t = default(T);
                t.Title = Path.GetFileName(Path.GetDirectoryName(path));
                t.State = MetadataState.BadJson;
            }

            return t;
        }

        public static MetadataBase Load(string path)
        {
            return MetadataBase.Load<MetadataBase>(path);
        }
    }
}