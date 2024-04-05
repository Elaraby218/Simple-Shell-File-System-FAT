using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static void quit()
        {
            FatTable.WriteFatTable();
            Virtual_Disk.fs.Close();
            Environment.Exit(0);
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

        public static void md(string dirName)
        {
            if (Program.CurrentDirectory.SearchDir(dirName) == -1)
            {
                Directory_Entry newDir = new Directory_Entry(dirName, 0x10, 0, 0);
                Program.CurrentDirectory.DirectoryTable.Add(newDir);           
                Program.CurrentDirectory.WriteDirectory();
         
               
                if (Program.CurrentDirectory.Parent != null)
                {
                    Program.CurrentDirectory.Parent.WriteDirectory();
                }

                Program.CurrentDirectory.WriteDirectory();
                Console.WriteLine($"Directory '{dirName}' created successfully");
            }
            else
            {
                Console.WriteLine($"Directory '{dirName}' already exists");
            }
        }

        public static void rd(string dirName)
        {
            int id = Program.CurrentDirectory.SearchDir(dirName);

            if (id != -1)
            {
                int FirstCluster = Program.CurrentDirectory.DirectoryTable[id].starting_cluster;
                int FileSize = Program.CurrentDirectory.DirectoryTable[id].size;

                // Create a new Directory object
                Directory directory = new Directory(dirName, 0x10,
                        FileSize, FirstCluster, Program.CurrentDirectory);

                // Delete the directory
                directory.DeleteDirectory(dirName); // Assuming DeleteDirectory doesn't need dirName argument
                Program.CurrentDirectory.WriteDirectory();

                Console.WriteLine($"Directory '{dirName}' deleted successfully");
            }
            else
            {
                Console.WriteLine($"Directory '{dirName}' not found");
            }
        }

        public static void dir()
        {
            Console.WriteLine();
            string name = " ";
            int NumOfFiles = 0, NumOfFolders = 0, Totalsize = 0;
            for (int i = 0; i < Program.CurrentDirectory.DirectoryTable.Count; i++)
            {
                if (Program.CurrentDirectory.DirectoryTable[i].attribute == 0x10)
                {
                    name = new string(Program.CurrentDirectory.DirectoryTable[i].name);
                    Console.WriteLine($"{name,-50} <DIR>");
                    NumOfFolders++;
                }
                else
                {
                    name = new string(Program.CurrentDirectory.DirectoryTable[i].name);
                    Console.WriteLine($"{name,-50} {Program.CurrentDirectory.DirectoryTable[i].size} bytes");
                    NumOfFiles++;
                    Totalsize += Program.CurrentDirectory.DirectoryTable[i].size;
                }
            }
            Console.WriteLine("");
            Console.WriteLine($"<DIR(s)> {NumOfFolders,-10} File(s) {NumOfFiles,-10}  {Totalsize}Bytes");
            Console.WriteLine("");

        }

        public static void cd(string dirName)
        {
            if(dirName == "..")
            {
               
                if(Program.CurrentDirectory.Parent != null)
                {
                    Program.CurrentDirectory = Program.CurrentDirectory.Parent;
                    Program.Path = Program.Path.Substring(0, Program.Path.LastIndexOf("\\"));
                }
                else
                {
                    Console.WriteLine("You are in the root directory");
                }
                return;
            }

            if (dirName.Contains('\\'))
            {
                List<string> path = dirName.Split('\\').ToList();
                Directory curDir; 

                if (dirName == "\\")
                {
                    Program.CurrentDirectory = Virtual_Disk.Root;
                    Program.Path = "root";
                    Program.CurrentDirectory.ReadDirectory();
                    return;
                }

                if (dirName[0] == '\\')
                {
                    curDir = Program.CurrentDirectory;             
                    path.RemoveAt(0);
                }
                else
                    curDir = Virtual_Disk.Root;

                curDir.ReadDirectory();
                string newpath = "";

                foreach( var item in path)
                {
                    int idx  = curDir.SearchDir(item);
                    if (idx == -1)
                    {
                        Console.WriteLine($"Directory '{item}' not found in the given Path ... ");
                        return;
                    }
                    Directory d = new Directory(item, 0x10, curDir.DirectoryTable[idx].size, 
                                                              curDir.DirectoryTable[idx].starting_cluster, curDir);
                    curDir = d;
                    curDir.ReadDirectory();
                    newpath += "\\";
                    newpath += item;
                }
                Program.CurrentDirectory = curDir;
                Program.Path += newpath;
                Program.CurrentDirectory.ReadDirectory();
                return;
            }

            int id = Program.CurrentDirectory.SearchDir(dirName);
            if (id != -1)
            {
                int FirstCluster = Program.CurrentDirectory.DirectoryTable[id].starting_cluster;
                int FileSize = Program.CurrentDirectory.DirectoryTable[id].size;

                Directory directory = new Directory(dirName, 0x10,
                                           FileSize, FirstCluster, Program.CurrentDirectory);

                Program.CurrentDirectory = directory;
                Program.Path += "\\";
                Program.Path += dirName;
                Program.CurrentDirectory.ReadDirectory();
            }
            else
            {
                Console.WriteLine($"Directory '{dirName}' not found");
            }
        }

    }
}
