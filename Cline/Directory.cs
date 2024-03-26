using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cline
{
    internal class Directory : Directory_Entry
    {

        public List<Directory_Entry> DirectoryTable;                                // List of Directory Entries
        public Directory Parent = null;                                             // Parent Directory


        public Directory(string name, byte attribute, int size, int starting_cluster , Directory Parent)
            : base(name, attribute, size, starting_cluster)
        {
            DirectoryTable = new List<Directory_Entry>();
            if(Parent != null)
            {
                this.Parent = Parent;
            }
        }

        // static to be shared over all the instances of the class
        public static List<byte[]> PrepData(byte[] data)
        {
            List<byte[]> ReadyData = new List<byte[]>(); 
            int FullBlocks = data.Length / 1024; 
            int RemBytes = data.Length % 1024;
            for(int i = 0; i < FullBlocks; i++)
            {
                byte[] temp = new byte[1024];
                for (int j = 0; j < 1024; j++)
                {
                    temp[j] = data[i * 1024 + j];
                }
                ReadyData.Add(temp);
            }
            if(RemBytes > 0)
            {
                byte[] temp = new byte[1024];
                for (int i = 0; i < RemBytes; i++)
                {
                    temp[i] = data[FullBlocks * 1024 + i];
                }
                ReadyData.Add(temp);
            }
            return ReadyData;
        }   
        public void WriteDirectory()
        {
            byte[] DirsOrFIleBytes = new byte[DirectoryTable.Count*32]; // elgardel 32 bytes for each entry
            int CurrentCluster = this.starting_cluster;
            int LastCluster = -1;

            // fill dirorfilebytes with the directory entries in bytes
            for(int i = 0; i < DirectoryTable.Count; i++)
            {
                byte[] temp = DirectoryTable[i].DirectoryEntryToByte();
                for (int j = 0; j < 32; j++)
                {
                    DirsOrFIleBytes[i * 32 + j] = temp[j];
                }
            }

            List<byte[]> Data = PrepData(DirsOrFIleBytes);

            // determine the starting cluster
            if(this.starting_cluster !=0)
            {
                CurrentCluster = this.starting_cluster;
            }
            else
            {
                CurrentCluster = FatTable.First_Ava_Block();
                this.starting_cluster = CurrentCluster;
                // if there is no available blocks in the fat table it will return -1 
            }

            // till now we have the data in the form of list of byte arrays
            // we know the first cluster we will write to
            // now we will write the data to the disk

            for(int i = 0; i < Data.Count; i++)
            {
                if (CurrentCluster == -1)
                {
                    Console.WriteLine("No available blocks in the FAT table");
                    return;
                }
                Virtual_Disk.WriteBlock(Data[i] , CurrentCluster);
                FatTable.SetVal(CurrentCluster, -1); // informe that the used block is end here 

                if(LastCluster != -1)
                {
                    FatTable.SetVal(CurrentCluster, LastCluster);
                }

                LastCluster = CurrentCluster;
                CurrentCluster = FatTable.First_Ava_Block();
            }

            // update the parent directory

            FatTable.WriteFatTable();

        }




       
    }
    
    
}
