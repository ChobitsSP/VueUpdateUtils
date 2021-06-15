using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VueUpdateUtils.Tasks;

namespace VueUpdateUtils
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("input dir:");
                var dir = Console.ReadLine();

                var dic = new Dictionary<string, BaseTask>();

                dic["AddVueExtension"] = new AddVueExtension() { ext = "*.vue" };
                dic["RemoveUnUsedImport"] = new RemoveUnUsedImport();
                dic["ReplaceOldImport"] = new ReplaceOldImport();

                var task = dic["ReplaceOldImport"];

                task.rootDir = dir;

                var count = task.Run();

                Console.WriteLine("success! total count: {0}", count);
            }
        }
    }
}
