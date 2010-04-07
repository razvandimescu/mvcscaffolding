using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace console
{
    public class ConstsReplacer
    {
        private Dictionary<string, string> macros = new Dictionary<string, string>();
        public ConstsReplacer(string projectName)
        {
            macros.Add("___appname___", projectName);
            macros.Add("___sollutionGUID___", Guid.NewGuid().ToString().ToUpper());
            macros.Add("___website_csproj_GUID___", Guid.NewGuid().ToString().ToUpper());
        }

        public string replace(string text)
        {
            foreach (string key in macros.Keys)
            {
                if (text.Contains(key))
                {
                    text = text.Replace(key, macros[key]);
                }
            }

            return text;
        }
    }
}
