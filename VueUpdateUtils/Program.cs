using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                var files = Directory.GetFiles(dir, "*.vue", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    StringUtils.AddVueExtension(dir, file);
                }

                Console.WriteLine("success!");
            }
        }
    }
}
