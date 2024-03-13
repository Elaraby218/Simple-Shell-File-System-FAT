using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cline
{
    internal static class Execute
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
            {"quit", "Exit the Console"},
            {"cd", "Change the current default directory"},
            {"copy", "Copies one or more files to another location"},
            {"del", "Deletes one or more files"},
            {"md", "Creates a directory"},
            {"rd", "Remove directory"},
            {"rename", "Renames a file"},
            {"type", "Displays the contents of a text file"},
            {"import", "Improt text file from your computer"},
            {"export", "export text file to your computer"}};

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
