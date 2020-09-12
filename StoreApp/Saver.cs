using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StoreApp
{
    public static class Saver
    {
        public static void Write( string file, string data)
        {
            using (FileStream fs = File.OpenWrite(file))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                // Put count.
                writer.Write(data);
                
                fs.Close();
            }
        }

        public static string Read(string file)
        {
            string result = "";
            using (FileStream fs = File.OpenRead(file))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                // Get count.
                result = reader.ReadString();
                reader.Close();
            }
            return result;
        }
    }
}
