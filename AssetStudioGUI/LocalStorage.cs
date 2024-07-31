using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace AssetStudioGUI
{
    public class LocalStorage
    {
        private static Dictionary<string, string> dic;
        private static string prefJson = "pref.json";

        public static void SetString(string key, string val)
        {
            if (dic == null)
                dic = new Dictionary<string, string>();

            dic[key] = val;

            // string serialize = JsonSerializer.Serialize(dic);
            // File.WriteAllText("pref.json", serialize);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonTextWriter jsonTextReader = new JsonTextWriter(sw);
            new JsonSerializer().Serialize(jsonTextReader, dic);
            File.WriteAllText(prefJson, sb.ToString());
        }

        public static string GetString(string key, string defVal)
        {
            if (dic == null)
            {
                if (File.Exists(prefJson))
                {
                    // string readAllText = File.ReadAllText("pref.json");
                    // dic = JsonSerializer.Deserialize<Dictionary<string, string>>(readAllText);

                    StreamReader streamReader = File.OpenText(prefJson);
                    JsonTextReader jsonTextReader = new JsonTextReader(streamReader);
                    dic = new JsonSerializer().Deserialize<Dictionary<string, string>>(jsonTextReader);
                    jsonTextReader.Close();
                }
            }

            if (dic != null)
            {
                string outstr;
                if (dic.TryGetValue(key, out outstr))
                {
                    return outstr;
                }
            }

            return defVal;
        }
    }
}