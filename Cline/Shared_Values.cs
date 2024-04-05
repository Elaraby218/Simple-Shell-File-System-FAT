using System;
using System.Collections.Generic;
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
          //  if (Shared_Values.Arguments.Count == 0)
          //  {
                if (Command == "cls")  { Execute.cls() ; return; }
                if (Command == "help") { Execute.help(); return; }
                if (Command == "quit") { Execute.quit(); return; }
                if (Command == "dir")  { Execute.dir() ; return; }
          //      Console.WriteLine("No arguments provided");
           //     return;
         //   }
            if (Command == "md") Execute.md(Shared_Values.Arguments[0].ToString());
            if (Command == "rd") Execute.rd(Shared_Values.Arguments[0].ToString());
            if (Command == "cd") Execute.cd(Shared_Values.Arguments[0].ToString());
        }

    }
}
