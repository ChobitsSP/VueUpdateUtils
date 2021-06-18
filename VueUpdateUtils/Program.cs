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

                dic["AddVueExtension"] = new AddVueExtension();
                // dic["RemoveUnUsedImport"] = new RemoveUnUsedImport();
                dic["ReplaceOldImport"] = new ReplaceOldImport();

                foreach (var kv in dic)
                {
                    var task = kv.Value;
                    task.rootDir = dir;
                    var count = task.Run();
                    Console.WriteLine("{0} success! total count: {1}", kv.Key, count);
                }
            }
        }
    }
}
