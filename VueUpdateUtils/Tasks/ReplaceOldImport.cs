using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VueUpdateUtils.Tasks
{
    public class ReplaceOldImport : BaseTask
    {
        public override string[] ext_list
        {
            get
            {
                return new string[] { "*.js" };
            }
        }

        protected override bool Execute(string filePath)
        {
            Regex reg1 = new Regex(@"resolve =>[ \r\n]+require\(\[([^\]]+)\], resolve\)");
            var str = File.ReadAllText(filePath, encoding);
            if (!reg1.IsMatch(str)) return false;

            str = reg1.Replace(str, m =>
            {
                return string.Format(@"() => import({0})", m.Groups[1].Value);
            });

            File.WriteAllText(filePath, str, encoding);
            return true;
        }
    }
}
