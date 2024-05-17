using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cline
{
    internal static class Shared_Values
    {

        public static string Command;
        public static List<string> Arguments = new List<string>();

        public static Dictionary<string, List<string>> Commands_Args = new Dictionary<string, List<string>>();
        static public void ini()
        {
            List<string> args = new List<string>();
            Commands_Args.Add("cls", args);
            args.Add("cls");
            args.Add("help");
            args.Add("quit");
            args.Sort();

            Commands_Args.Add("help", args);
            args.Clear();

            Commands_Args.Add("quit", args);
            args.Clear();

            Commands_Args.Add("md", args);
            args.Clear();

            Commands_Args.Add("rd", args);
            args.Clear();

            Commands_Args.Add("dir", args);
            args.Clear();

            Commands_Args.Add("cd", args);
            args.Clear();

            Commands_Args.Add("import", args);
            args.Clear();

            Commands_Args.Add("export", args);
            args.Clear();

            Commands_Args.Add("del", args);
            args.Clear();

            Commands_Args.Add("type", args);
            args.Clear();

            Commands_Args.Add("rename", args);
            args.Clear();

            Commands_Args.Add("copy", args);
            args.Clear();



        }

        public static void Rmv_spcs(string inputt_)
        {
            string[] parts = inputt_.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                Shared_Values.Arguments.Add(part.Trim());
            }
            Shared_Values.Command = Shared_Values.Arguments[0];
        }

        public static bool Is_Command(string comm)
        {
            if (Commands_Args.ContainsKey(comm))
                return true;
            return false;
        }
        public static bool Is_arguments(List<string> arg)
        {
            List<string> Truearguments = Commands_Args[Command].ToList();
            bool allPresent = Truearguments.All(item => arg.Contains(item));
            if (allPresent) return true;
            return false;
        }
        public static void ExcuteCommand()
        {


            if (Command == "cls") { Execute.cls(); return; }
            if (Command == "help") { Execute.help(); return; }
            if (Command == "quit") { Execute.quit(); return; }
            if (Command == "dir") { Execute.dir(); return; }

            bool IsArgFound = (Shared_Values.Arguments.Count > 0);
            bool TwoArgs = (Shared_Values.Arguments.Count == 2);
            string firstarg = ((Command == "export") ? "Src" : (Command == "copy") ? "Copy Name" : "Old Name"),
                  secondarg = ((Command == "export") ? "Dest" : (Command == "copy") ? "Destination" : "New Name");

            if (Command == "md" && IsArgFound) { Execute.md(Shared_Values.Arguments[0].ToString());  return; }

            if (Command == "rd" && IsArgFound) { Execute.rd(Shared_Values.Arguments[0].ToString());  return; }

            if (Command == "cd" && IsArgFound) { Execute.cd(Shared_Values.Arguments[0].ToString()); return; }

            if (Command == "import" && IsArgFound) { Execute.import(Shared_Values.Arguments[0].ToString()); return; }

            if (Command == "export" && TwoArgs) { Execute.export(Shared_Values.Arguments[0].ToString(), Shared_Values.Arguments[1].ToString()); return; }

            if (Command == "del" && IsArgFound) { Execute.del(Shared_Values.Arguments[0].ToString()); return;  }

            if (Command == "type" && IsArgFound) { Execute.type(Shared_Values.Arguments[0].ToString()); return; }

            if (Command == "rename" && TwoArgs) { Execute.rename(Shared_Values.Arguments[0].ToString(), Shared_Values.Arguments[1].ToString()); return; }

            if (Command == "copy" && IsArgFound) { Execute.copy(Shared_Values.Arguments[0].ToString()); return; }
            if (!IsArgFound && !TwoArgs && Command != "export" && Command != "rename")
            {
                Console.WriteLine("This command is require an argument ...");
            }
            else
            {
                Console.WriteLine($"This command requires two args '{firstarg}' '{secondarg}' ...");
            }
            //copy : copy files and directories
        }

        private static void openTxtFile()
        {
            string filePath = "J:\\c#\\command_line\\Cline\\Cline\\bin\\Debug\\net8.0\\Data.txt"; // Replace this with the path to your file

            try
            {
                // Open the file using the default application
                Process.Start("notepad.exe", filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }

    }
}
