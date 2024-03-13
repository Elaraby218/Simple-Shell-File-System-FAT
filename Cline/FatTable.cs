using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cline
{
    internal static class FatTable
    {
        public static int[] Fat_Table = new int[1024];
        private static string filePath = Environment.CurrentDirectory + "\\Data.txt";
        public static void Initialize()
        {
            Fat_Table[0] = -1; // super block
            Fat_Table[1] =  2; // fat table 
            Fat_Table[2] =  3; // fat table 
            Fat_Table[3] =  4; // fat table 
            Fat_Table[4] = -1; // fat table 
            for(int i=5; i<Fat_Table.Length; i++)
            {
                Fat_Table[i] = 0;
            }
   
        }

        public static void WriteFatTable()
        {
            try
            {
                byte[] fatTableBytes = new byte[1024*4];
                Buffer.BlockCopy(Fat_Table, 0, fatTableBytes, 0, fatTableBytes.Length);

                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fs.Seek(1024, SeekOrigin.Begin);
                    // Write the FAT table data into the file
                    fs.Write(fatTableBytes, 0, fatTableBytes.Length);
                }
                Console.WriteLine("FAT table data has been written to the virtual disk.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public static void ReadFatTable()
        {
            try
            {
                int fatTableBytes = 1024 * 4;

                // Create an array of bytes to hold the FAT table data
                byte[] fatTableBytesArray = new byte[fatTableBytes];

                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {   
                    fs.Seek(1024, SeekOrigin.Begin); // skip super block
                    fs.Read(fatTableBytesArray, 0, fatTableBytes);
                }
                // Convert the byte array to an integer array using Buffer.BlockCopy    
                Buffer.BlockCopy(fatTableBytesArray, 0, Fat_Table, 0, fatTableBytes);
                for(int i=0; i<Fat_Table.Length; i++)
                {
                    Console.WriteLine(Fat_Table[i]);
                }
                Console.WriteLine("FAT table data has been read from the virtual disk.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public static void PrintFatTable()
        {
            foreach(int item in Fat_Table)
            {
                Console.Write($"{item} ");
            }
        }
        public static int First_Ava_Block()
        {
            for(int i=0; i < Fat_Table.Length; i++)
            {
                if (Fat_Table[i] == 0)
                    return i; 
            }
            return -1;
        }

        public static int GetVal(int idx)
        {
            return Fat_Table[idx];
        }

        public static void SetVal(int val , int idx)
        {
            Fat_Table[idx] = val;
        }

        public static int GetFreeBlocksNums()
        {
            return (Fat_Table.Count(num => num == 0));
        }

        public static int GetFreeSpace()
        {
            return (GetFreeBlocksNums()*1024);
        }
    }
}
