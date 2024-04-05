using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cline
{
    internal class Directory : Directory_Entry
    {

        public List<Directory_Entry> DirectoryTable;                                // List of Directory Entries
        public Directory Parent = null;                                             // Parent Directory


        public Directory(string name, byte attribute, int size, int starting_cluster, Directory Parent)
                                                                  : base(name, attribute, size, starting_cluster)
        {
            DirectoryTable = new List<Directory_Entry>();
            if (Parent != null)
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
            for (int i = 0; i < FullBlocks; i++)
            {
                byte[] temp = new byte[1024];
                for (int j = 0; j < 1024; j++)
                {
                    temp[j] = data[i * 1024 + j];
                }
                ReadyData.Add(temp);
            }
            if (RemBytes > 0)
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
            byte[] dummy = new byte[1024];
            for (int i = 0; i < 1024; i++)
            {
                dummy[i] = (byte)'#';
            }
            Virtual_Disk.WriteBlock(dummy, this.starting_cluster);
            byte[] DirsOrFIleBytes = new byte[DirectoryTable.Count * 32]; // elgardel 32 bytes for each entry
            int CurrentCluster = this.starting_cluster;
            int LastCluster = -1;


            for (int i = 0; i < this.DirectoryTable.Count; i++)
            {
                if (this.DirectoryTable[i].starting_cluster == 0)
                {
                    this.DirectoryTable[i].starting_cluster = FatTable.First_Ava_Block();
                    FatTable.SetVal(this.DirectoryTable[i].starting_cluster, -1);
                    FatTable.WriteFatTable();
                }
                byte[] temp = DirectoryTable[i].DirectoryEntryToByte();
                for (int j = 0; j < 32; j++)
                {
                    DirsOrFIleBytes[((i) * 32 + j)] = temp[j];
                }
            }

            // if (DirsOrFIleBytes.Length == 0)
            // {
            //     return;
            // }

            List<byte[]> Data = PrepData(DirsOrFIleBytes);

            // determine the starting cluster
            
           

            // till now we have the data in the form of list of byte arrays
            // we know the first cluster we will write to
            // now we will write the data to the disk

            for (int i = 0; i < Data.Count; i++)
            {
                if (CurrentCluster == -1)
                {
                    Console.WriteLine("No available blocks in the FAT table");
                    return;
                }

                Virtual_Disk.WriteBlock(Data[i], CurrentCluster);
                FatTable.SetVal(CurrentCluster, -1); // informe that the used block is end here 

                if (LastCluster != -1)
                {
                    FatTable.SetVal(LastCluster, CurrentCluster);
                }

                LastCluster = CurrentCluster;
                CurrentCluster = FatTable.First_Ava_Block();
            }
            FatTable.WriteFatTable();
        }

        public void UpdateParent(Directory_Entry old)
        {
            int idx = this.SearchDir(new string(old.name));
            if (idx != -1)
            {
                DirectoryTable.RemoveAt(idx);
                DirectoryTable.Insert(idx, old);
            }
        }

        public void printtable()
        {
            Console.WriteLine("hi");
            foreach (var item in DirectoryTable)
            {
                Console.WriteLine(item.name);
            }
        }
        public void ReadDirectory()
        {
            if (this.starting_cluster != 0)
            {


                int CurrentCluster = this.starting_cluster;
                List<byte> List_OF_Bytes = new List<byte>();
                List<Directory_Entry> DT = new List<Directory_Entry>();

                while (CurrentCluster != -1)
                {
                    // Read the block corresponding to the current cluster
                    List_OF_Bytes.AddRange(Virtual_Disk.ReadBlock(CurrentCluster));
                    // Get the next cluster from the FAT table
                    int NextCluster = FatTable.GetVal(CurrentCluster);
                    // Move to the next cluster
                    CurrentCluster = NextCluster;
                }

                // Parse directory entries from the read bytes
                for (int i = 0; i < List_OF_Bytes.Count; i += 32)
                {
                    byte[] temp = new byte[32];
                    Array.Copy(List_OF_Bytes.ToArray(), i, temp, 0, 32);

                    // Assuming ByteToDirectoryEntry method is defined elsewhere
                    if (temp[0] == '#' || temp[0] == 0)
                    {
                        break;
                    }
                    Directory_Entry entry = ByteToDirectoryEntry(temp);
                    DT.Add(entry);
                }
                this.DirectoryTable = DT;
            }

        }
        public int SearchDir(string name)
        {
            // Ensure the name is exactly 11 characters long
            name = name.PadRight(11).Substring(0, 11);

            // Search for the name in the directory table
            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                if (DirectoryTable[i].name.SequenceEqual(name))
                {
                    return i;
                }
            }
            return -1;
        }

        public int SearchFile(string name)
        {
            // Ensure the name is exactly 11 characters long
            name = name.PadRight(11).Substring(0, 11);

            // Search for the name in the directory table
            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                string n = new string(DirectoryTable[i].name);
                if (n == name)
                    return i;
            }
            return -1;
        }


        //public void DeleteDirectory(string DirName)
        //{
        //    if (this.Parent != null)
        //    {
        //        int idx = this.Parent.SearchDir(new string(this.name));
        //        if (idx != -1)
        //        {
        //            this.Parent.DirectoryTable.RemoveAt(idx);
        //            this.Parent.WriteDirectory();
        //        }
        //    }

        //    //Console.WriteLine(this.starting_cluster);
        //    FatTable.SetVal(this.starting_cluster, 0);
        //    if (this.DirectoryTable.Count == 0)
        //    {
        //        FatTable.SetVal(this.starting_cluster, 0);
        //        this.WriteDirectory();
        //        return;
        //    }

        //    foreach (var dire in this.DirectoryTable)
        //    {
        //        if (dire.attribute == 0x10)
        //        {
        //            Directory directory = new Directory(new string(dire.name), dire.attribute, dire.size, dire.starting_cluster, this);
        //            int CurrentCluster = this.starting_cluster;
        //            int NextCluster = -1;
        //            if (CurrentCluster != 0)
        //            {
        //                do
        //                {
        //                    NextCluster = FatTable.GetVal(CurrentCluster);
        //                    FatTable.SetVal(CurrentCluster, 0);
        //                    CurrentCluster = NextCluster;

        //                } while (NextCluster != -1);
        //            }
        //            directory.DeleteDirectory(new string(dire.name));
        //            directory.WriteDirectory();
        //        }

        //        // if it was file handle it later 
        //    }
        //    FatTable.WriteFatTable();

        //    // Delete the directory entries from the parent directory
        //    // Delete subdirectories and files within this directory (you need to implement this)


        //    // Update the FAT table and write it back to disk

        //}


        public void DeleteDirectory(string DirNameD)
        {
            this.ReadDirectory();
            if (this.DirectoryTable.Count > 0)
            {
                for (int i = 0; i < this.DirectoryTable.Count; i++)
                {
                    Directory todel = new Directory(new string(this.DirectoryTable[i].name),
                                                                   this.DirectoryTable[i].attribute,
                                                                                    this.DirectoryTable[i].size,
                                                                                                this.DirectoryTable[i].starting_cluster, this);
                    if (todel.attribute == 0x10)
                    {
                        todel.DeleteDirectory(new string(todel.name));
                    }
                }
            }
            if (this.Parent != null)
            {
                int idx = this.Parent.SearchDir(new string(this.name));
                if (idx != -1)
                {
                    this.Parent.DirectoryTable.RemoveAt(idx);
                    this.Parent.WriteDirectory();
                }
            }

            Virtual_Disk.WriteBlock(Virtual_Disk.EmptyBlock, this.starting_cluster);
            FatTable.SetVal(this.starting_cluster, 0);
            FatTable.WriteFatTable();
           
            return;
        }


        //public void DeleteDirectory(string DirNameD)
        //{
        //    // Delete subdirectories recursively
        //    for (int i = 0; i < this.DirectoryTable.Count; i++)
        //    {
        //        Directory todel = new Directory(new string(this.DirectoryTable[i].name),
        //                                         this.DirectoryTable[i].attribute,
        //                                         this.DirectoryTable[i].size,
        //                                         this.DirectoryTable[i].starting_cluster, this);
        //        if (todel.attribute == 0x10)
        //        {
        //            todel.DeleteDirectory(new string(todel.name));
        //        }
        //    }

        //    // Remove this directory entry from parent directory
        //    if (this.Parent != null)
        //    {
        //        int idx = this.Parent.SearchDir(new string(this.name));
        //        if (idx != -1)
        //        {
        //            this.Parent.DirectoryTable.RemoveAt(idx);
        //            this.Parent.WriteDirectory();
        //        }
        //    }

        //    // Free up cluster space occupied by this directory
        //    FatTable.SetVal(this.starting_cluster, 0);
        //    Virtual_Disk.WriteBlock(Virtual_Disk.EmptyBlock, this.starting_cluster);
        //    FatTable.WriteFatTable();
        //}

    }
}
