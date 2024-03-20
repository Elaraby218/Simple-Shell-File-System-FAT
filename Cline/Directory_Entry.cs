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

        public Directory_Entry(string name, byte attribute, int size, int starting_cluster)
        {
            this.attribute = attribute;
            this.size = size;
            this.starting_cluster = ( (starting_cluster == 0) ? FatTable.First_Ava_Block() : starting_cluster );

            if (this.attribute == 0)
            {
                this.name = (name.Length > 11) ?
                            (name.Substring(0,7) + name.Substring(name.Length - 4)).ToCharArray() :
                            name.ToCharArray();
            }
            else
            {
                this.name = name.Substring(0, Math.Min(name.Length, 11)).ToCharArray();
            }
        }


        public byte[] Convert_Directory_Entry()
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

       public Directory_Entry Convert_Byte_To_Directory_Entry(byte[] data)
        {


            return new Directory_Entry
            {
                name = Encoding.ASCII.GetString(data, 0, 11).ToCharArray(),
                attribute = data[11],
                size = BitConverter.ToInt32(data, 24),
                starting_cluster = BitConverter.ToInt32(data, 28),
            };
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
