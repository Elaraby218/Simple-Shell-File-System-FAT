﻿using System;
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
        public static List<string> Arguments = new List<string>() ;

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
            if (Command == "cls") Execute.cls();
            if (Command == "help") Execute.help();
            if (Command == "quit") Execute.quit();
            if (Command == "md") Execute.md(Shared_Values.Arguments[0].ToString());
        }
       
    }
}
