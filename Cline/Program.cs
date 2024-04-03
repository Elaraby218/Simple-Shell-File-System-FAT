using System.Security.Cryptography.X509Certificates;

namespace Cline
{


    internal class Program
    {
        public static Directory CurrentDirectory;
        public static string  Path = "root" ; 
        
        public static void Main(string[] args)
        {
            
            Shared_Values.ini();
            Virtual_Disk.Initialize();
            CurrentDirectory.printtable();
            FatTable.PrintFatTable();
            while (true)
            {
                Console.Write(Path + ":\\> ");
                string inputt_ = Console.ReadLine();
                if (string.IsNullOrEmpty(inputt_) || string.IsNullOrWhiteSpace(inputt_))  continue;
                Shared_Values.Rmv_spcs(inputt_); 
                
                if (Shared_Values.Command == "quit")
                {
                   Shared_Values.ExcuteCommand();
                }
                else
                {
                    if (Shared_Values.Is_Command(Shared_Values.Command))
                    {
                        Shared_Values.Arguments.RemoveAt(0);   
                        if (Shared_Values.Is_arguments(Shared_Values.Arguments))
                        {
                            Shared_Values.ExcuteCommand();
                        }
                        else
                        {
                            Console.WriteLine("Wrong arguments list... ");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot recoginze the command, type help for more informations... ");
                    }
                }
                Shared_Values.Command = "";
                Shared_Values.Arguments.Clear();

            }
        }

        static public string Cur_location()
        {
            string cur = Environment.CurrentDirectory;
            return cur;
        }

       
    }
}
