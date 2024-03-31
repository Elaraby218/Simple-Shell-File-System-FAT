using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
------------------------------------------------------------------------------------------------------------------------
| Filename (11 bytes) | FileAttribute (1 byte) | FileEmpty (12 bytes) | FirstCluster (4 bytes) | FileSize (4 bytes)    |
------------------------------------------------------------------------------------------------------------------------
|        Name [0,10]  |         Attr [11]      |   12 zeros [12,23]  | Cluster Number [24,27] | Size in Bytes [28,31]  |
------------------------------------------------------------------------------------------------------------------------
*/

namespace Cline
{
    internal class Directory_Entry
    {
        public char[] name = new char[11];
        public byte attribute; 
        public byte[] empty = new byte[12];
        public int size; 
        public int starting_cluster;

        public Directory_Entry(){  }

        public void DirName(char[] name)
        {
            int maxLength = 11; 
            int nameLength = Math.Min(name.Length, maxLength);
            Array.Copy(name, this.name, nameLength);
            // Fill the remaining space with spaces
            for (int i = nameLength; i < maxLength; i++)
            {
                this.name[i] = ' ';
            }
        }

        public void FileName(char[] name, char[] ext)
        {
            int maxLength = 8; // Maximum length for DOS filenames
            int extLength = 3; // Length of file extension

            if (name.Length <= 7 && ext.Length == 3)
            {
                Array.Copy(name, 0, this.name, 0, Math.Min(name.Length, maxLength - extLength - 1));
                this.name[Math.Min(name.Length, maxLength - extLength - 1)] = '.';
                Array.Copy(ext, 0, this.name, Math.Min(name.Length, maxLength - extLength), Math.Min(ext.Length, extLength));
            }
            else
            {
                Array.Copy(name, 0, this.name, 0, Math.Min(name.Length, maxLength - extLength - 1));
                this.name[Math.Min(name.Length, maxLength - extLength - 1)] = '.';
                Array.Copy(ext, 0, this.name, maxLength - extLength, Math.Min(ext.Length, extLength));
            }

            // Fill the remaining space with spaces
            for (int i = name.Length + ext.Length; i < maxLength; i++)
            {
                this.name[i] = ' ';
            }
        }

        // i changed the funcitno of name to fill the empty cells with spaces 
        public Directory_Entry(string name, byte attribute, int size, int starting_cluster)
        {
            this.attribute = attribute;
            this.size = size;
            this.starting_cluster = ( (starting_cluster == 0) ? FatTable.First_Ava_Block() : starting_cluster );

            if (this.attribute == 0) // file 
            {
                string[] filename = name.Split('.');
                FileName(filename[0].ToCharArray(), filename[1].ToCharArray());
            }
            else
            {
                DirName(name.ToCharArray());
            }
        }


        public byte[] DirectoryEntryToByte()
        {
            byte[] data = new byte[32];

            
            // 0 : 10 is the name of the file
            int nameLength = Math.Min(11,this.name.Length) ;
            for (int i = 0; i < nameLength; i++)
            {
                data[i] = (byte)this.name[i];
            }

            // 11 is the attribute of the file
            data[11] = this.attribute;

            // 12 : 23 is the empty space
            Array.Copy(this.empty, 0, data, 12, Math.Min(this.empty.Length, 12));

            // 24 : 27 is the size of the file
            Array.Copy(BitConverter.GetBytes(this.size), 0, data, 24, 4);

            // 28 : 31 is the starting cluster of the file
            Array.Copy(BitConverter.GetBytes(this.starting_cluster), 0, data, 28, 4);

            return data;
        }

       public Directory_Entry ByteToDirectoryEntry(byte[] data)
        {


            return new Directory_Entry
            {
                name = Encoding.ASCII.GetString(data, 0, 11).ToCharArray(),
                attribute = data[11],
                size = BitConverter.ToInt32(data, 24),
                starting_cluster = BitConverter.ToInt32(data, 28),
            };
        }

        public Directory_Entry GetCur()
        {
            return this; 
        }

        public void ToString()
        {
            Console.WriteLine($"Name: {new string(this.name)} + {this.name.Length}");
            Console.WriteLine($"Attribute: {this.attribute}");
            Console.WriteLine($"Size: {this.size}");
            Console.WriteLine($"Starting Cluster: {this.starting_cluster}");
        }

        
    }
}
