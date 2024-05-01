using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cline
{
    internal class FileEntry : Directory_Entry
    {
        public Directory Parent = null;
        public string content;

        // constructor
        public FileEntry(string name, byte attribute, int size, int starting_cluster, Directory Parent , string content)
            : base(name, attribute, size, starting_cluster)
        {
            this.Parent = Parent;
            this.content = content;
        }

        // function to convert content to bytes to serve writing to the disk
        public byte[] ContentToBytes(string content)
        {
            byte[] ContentInBytes = Encoding.UTF8.GetBytes(content);
            return ContentInBytes;
        }

        // function to convert bytes to string to serve reading from the disk
        public string BytesToContent(byte[] content)
        {
            string ContentInString = Encoding.UTF8.GetString(content);
            return ContentInString;
        }

        // function to write the file content to the disk
        public void WriteFile()
        {
            byte[] contentInBytes = ContentToBytes(content);
            List<byte[]> ReadyData = Directory.PrepData(contentInBytes);
            int cluster = this.starting_cluster;
            int i = 0;
            while (i < ReadyData.Count)
            {
                Virtual_Disk.WriteBlock(ReadyData[i], cluster);
                FatTable.SetVal(cluster, cluster + 1);
                cluster = FatTable.First_Ava_Block();
                i++;
            }
            FatTable.SetVal(cluster, -1);
            FatTable.WriteFatTable();
        }

        // function to read the file content from the disk
        public void ReadFile()
        {
            byte[] contentInBytes = new byte[this.size];
            List<byte[]> ReadyData = new List<byte[]>(); // it will contain every block data , list of blocks
            int cluster = this.starting_cluster;
            do
            {
                contentInBytes = Virtual_Disk.ReadBlock(cluster);
                ReadyData.Add(contentInBytes);
                cluster = FatTable.GetVal(cluster); // get the next clusetr that the data is stored in
            }
            while (cluster != -1);
            // convert list of bytes into one array of bytes then convert it into string using BytesToContent function
            this.content = BytesToContent(ReadyData.SelectMany(a => a).Where(b => b != '#').ToArray());
        }

        // fuctnion to delete the file from the disk
        public void DeleteFile()
        {
            int cluster = this.starting_cluster;
            int nextCluster;
            do
            {
                nextCluster = FatTable.GetVal(cluster);
                FatTable.SetVal(cluster, 0);
                // Delete the data from the disk because overriding may cause conflicts.
                Virtual_Disk.WriteBlock(Virtual_Disk.EmptyBlock, cluster);
                cluster = nextCluster;
            }
            while (cluster != -1);

            string FileName = new string(this.name);
            int idx = this.Parent.SearchDir(FileName);
            if(idx != -1)
            {
                this.Parent.DirectoryTable.RemoveAt(idx);
                this.Parent.WriteDirectory();
            }
           
            FatTable.WriteFatTable();
        }

        // function to show the content of the file 
        public void ShowContent()
        {
            Console.WriteLine(this.content); 
        }
    }
}
