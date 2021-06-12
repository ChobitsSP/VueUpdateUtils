using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VueUpdateUtils
{
    public static class StringUtils
    {
        // static Encoding encoding = new UTF8Encoding(false);
        static Encoding encoding = Encoding.UTF8;

        // resolve => require(["../views/Page1/Index.vue"], resolve),
        public static void ReplaceOldImport(string rootDir, string filePath)
        {
            Regex reg1 = new Regex(@"resolve =>[ \r\n]+require\(\[([^\]]+)\], resolve\)");
            var str = File.ReadAllText(filePath, encoding);
            if (!reg1.IsMatch(str)) return;

            str = reg1.Replace(str, m =>
            {
                return string.Format(@"() => import({0})", m.Groups[1].Value);
            });

            File.WriteAllText(filePath, str, encoding);
        }

        static bool CheckIsUsed(string str, string value, string name)
        {
            var newStr = Regex.Replace(str, value, string.Empty);
            return newStr.Contains(name);
        }

        const string ROOT_PREFIX = "@";

        public static void RemoveUnUsedImport(string rootDir, string filePath)
        {
            Regex reg1 = new Regex(@"import (\w+) from ['""]([^""']+)");
            var str = File.ReadAllText(filePath, encoding);
            var lines = File.ReadAllLines(filePath, encoding);

            var matchObjects = reg1.Matches(str);

            if (matchObjects.Count == 0) return;

            var regList = matchObjects.Cast<Match>().OrderByDescending(t => t.Index);

            foreach (Match m in regList)
            {
                var moduleName = m.Groups[1].Value;
                var modulePath = m.Groups[2].Value;

                if (CheckIsUsed(str, m.Value, moduleName) == false)
                {
                    lines = lines.Where(t => !t.Contains(m.Value)).ToArray();
                }
            }

            File.WriteAllLines(filePath, lines, encoding);
        }

        public static void AddVueExtension(string rootDir, string filePath, string ext = ".vue")
        {
            Regex reg1 = new Regex(@"import (\w+) from ['""]([^""']+)['""]");

            var str = File.ReadAllText(filePath, encoding);
            var lines = File.ReadAllLines(filePath, encoding);

            var matchObjects = reg1.Matches(str);

            if (matchObjects.Count == 0) return;

            var regList = matchObjects.Cast<Match>().OrderByDescending(t => t.Index);

            bool hasChange = false;

            foreach (Match m in regList)
            {
                var moduleName = m.Groups[1].Value;
                var modulePath = m.Groups[2].Value.Replace('/', '\\');

                if (modulePath.StartsWith(ROOT_PREFIX))
                {
                    modulePath = Path.Combine(rootDir, modulePath.Substring(2));

                    if (File.Exists(modulePath)) continue;

                    var VueFile = modulePath + ext;

                    if (File.Exists(VueFile))
                    {
                        for (var i = 0; i < lines.Length; i++)
                        {
                            var line = lines[i];

                            if (line.Contains(m.Value))
                            {
                                lines[i] = reg1.Replace(line, "import $1 from '$2'" + ext);
                                hasChange = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (hasChange)
            {
                File.WriteAllLines(filePath, lines, encoding);
            }
        }
    }
}
