using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VueUpdateUtils.Tasks
{
    public abstract class BaseTask
    {
        public string rootDir { get; set; }
        protected Encoding encoding = Encoding.UTF8;
        protected abstract bool Execute(string filePath);

        static string ROOT_PREFIX = "@";

        public abstract string[] ext_list { get; }

        public int Run()
        {
            int count = 0;

            foreach (var filePath in GetFiles(rootDir, ext_list))
            {
                var result = Execute(filePath);

                if (result)
                {
                    count++;
                }
            }

            return count;
        }

        public static IEnumerable<string> GetFiles(string dir, params string[] searchList)
        {
            foreach (var search in searchList)
            {
                var files = Directory.GetFiles(dir, search, SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    yield return file;
                }
            }
        }

        protected static string GetFileRealPath(string rootDir, string filePath, string modulePath)
        {
            var result = modulePath.Replace('/', '\\');

            if (result.StartsWith(ROOT_PREFIX))
            {
                result = Path.Combine(rootDir, result.Substring(2));
            }
            else
            {
                result = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePath), result));
            }

            return result;
        }

        protected static bool CheckIsUsed(string str, string value, string name)
        {
            var newStr = Regex.Replace(str, value, string.Empty);
            return newStr.Contains(name);
        }
    }
}
