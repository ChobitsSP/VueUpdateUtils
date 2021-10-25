using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VueUpdateUtils.Tasks
{
    public class CheckMpComPath : BaseTask
    {
        public override string[] ext_list
        {
            get
            {
                return new string[] { "*.json" };
            }
        }

        protected override bool Execute(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var jobj = JObject.Parse(json);

            if (!jobj.ContainsKey("usingComponents")) return false;

            var dic = jobj["usingComponents"].ToObject<Dictionary<string, string>>();

            foreach (var kv in dic)
            {
                if (!kv.Value.StartsWith(".")) continue;
                var jsPath = kv.Value + ".js";
                var jsRealPath = GetFileRealPath(rootDir, filePath, jsPath);
                if (File.Exists(jsRealPath)) continue;
                Console.WriteLine(filePath, jsPath);
            }

            return true;
        }
    }
}
