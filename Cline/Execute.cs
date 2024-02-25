using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cline
{
    internal class Execute
    {
        public static void cls()
        {
            Console.Clear();
        }

        public static void help()
        {
            // Static dictionary for holding help information
            Dictionary<string, string> helpDictionary = new Dictionary<string, string>{
                                     {"help", "Display information about the commands"},
                                     {"cls", "Clear the Console"},
                                     {"quit", "Exit the Console"}};

            Shared_Values.Arguments = Shared_Values.Arguments.Distinct().ToList(); // 
            if (Shared_Values.Arguments.Count == 0)
            {
                foreach (var kvp in helpDictionary)
                {
                    Console.WriteLine($"{kvp.Key,-10} --> {kvp.Value}");
                }
            }
            else
            {
                foreach (var arg in Shared_Values.Arguments)
                {
                    if (helpDictionary.TryGetValue(arg, out string description))
                    {
                        Console.WriteLine($"{arg,-10} --> {description}");
                    }
                    else
                    {
                        Console.WriteLine($"This argument '{arg}' is not recognized.");
                    }
                }
            }
        }

    }
}
