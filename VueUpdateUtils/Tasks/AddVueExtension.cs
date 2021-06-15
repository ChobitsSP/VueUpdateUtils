using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VueUpdateUtils.Tasks
{
    public class AddVueExtension : BaseTask
    {
        public string ext { get; set; } = ".vue";

        public override string[] ext_list
        {
            get
            {
                return new string[] { "*.vue" };
            }
        }

        protected override bool Execute(string filePath)
        {
            Regex reg1 = new Regex(@"import (\w+) from ['""]([^""']+)['""]");

            var str = File.ReadAllText(filePath, encoding);
            var lines = File.ReadAllLines(filePath, encoding);

            var matchObjects = reg1.Matches(str);

            if (matchObjects.Count == 0) return false;

            var regList = matchObjects.Cast<Match>().OrderByDescending(t => t.Index);

            bool hasChange = false;

            foreach (Match m in regList)
            {
                var moduleName = m.Groups[1].Value;
                var modulePath = GetFileRealPath(rootDir, filePath, m.Groups[2].Value);

                if (File.Exists(modulePath)) continue;

                var VueFile = modulePath + ext;

                if (!File.Exists(VueFile)) continue;

                for (var i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];

                    if (line.Contains(m.Value))
                    {
                        lines[i] = reg1.Replace(line, string.Format("import $1 from '$2{0}'", ext));
                        hasChange = true;
                        break;
                    }
                }
            }

            if (hasChange)
            {
                File.WriteAllLines(filePath, lines, encoding);
            }

            return hasChange;
        }
    }
}
