﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cline
{
    internal static class Virtual_Disk
    {
        private static string filePath = Environment.CurrentDirectory + "\\Data.txt";
        public  static FileStream fs;
        public static Directory Root;
        public static byte[] EmptyBlock = Encoding.ASCII.GetBytes(new string('#', 1024));
        private static void SetBlock(int NumB, char Chr)
        {
            byte[] Block0 = new byte[1024 * NumB];
            for (int i = 0; i < 1024 * NumB; i++)
            {
                Block0[i] = (byte)(Chr);
            }
            fs.Write(Block0, 0, Block0.Length);
        }
        public static void Initialize()
        {
            if (!File.Exists(filePath))
            {
                using (fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    SetBlock(1, '0'); // super block
                    SetBlock(4, '*'); // fat table               
                    SetBlock(1019, '#');
                    fs.SafeFileHandle.Close(); 
                }
                FatTable.Initialize();
                Root = new Directory("root", 0x10, 0, 5, null);
                Root.WriteDirectory();
                FatTable.SetVal(5, -1);
                FatTable.WriteFatTable();      
                Program.CurrentDirectory = Root;
            }
            else
            {

                FatTable.ReadFatTable();
                Root = new Directory("root", 0x10, 0, 5, null);
                Root.ReadDirectory();   // it will be implemented later
                Program.CurrentDirectory = Root;

            }
        }

        // takes array of bytes and write it on the disk(txt file) 
        public static void WriteBlock(byte[] data, int idx)
        {
            try
            {
                using (fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {

                    fs.Seek(idx * 1024, SeekOrigin.Begin);
                    //data = System.Text.Encoding.UTF8.GetBytes("Hi iam mohamed Elaraby iam tring to overwrite on the data"); 
                    fs.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

        }

        // read and return data 
        public static byte[] ReadBlock(int idx)
        {
            byte[] data = new byte[1024];
            using (fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fs.Seek(idx * 1024, SeekOrigin.Begin);
                fs.Read(data, 0, data.Length);
            }
            return data;
        }


    }
}
