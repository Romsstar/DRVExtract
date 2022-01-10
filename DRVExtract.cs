using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using DRV.Properties;

namespace DRVExtract
{
    class DRVExtract
    {
        public static string dir = "extracted//";
        public static string input = "";
        public static List<DRVEntry> GetDRVEntries(BinaryReader br, string dir)
        {
            byte[] drvfile;
            List<DRVEntry> entryList = new List<DRVEntry>();

            while (true)
            {
                DRVEntry entry = new DRVEntry(br);
                bool exists = System.IO.Directory.Exists(dir);
                System.IO.Directory.CreateDirectory(dir);
                if (entry.bitflag == 0)
                    break;
                entryList.Add(entry);
                if (entry.filesize != 0)
                    Console.WriteLine(entry.fullfileName + "| " + entry.offset + "| " + entry.filesize + "| " + entry.unk1 + "|");
            }

            foreach (var entry in entryList)
            {
                br.BaseStream.Seek(entry.offset, SeekOrigin.Begin);
                drvfile = br.ReadBytes(entry.filesize);

                if (entry.filesize != 0)
                {
                    File.WriteAllBytes(dir + entry.fullfileName, drvfile);
                }
                if (entry.bitflag == 0x80)
                {
                    string subdir = dir + entry.fileNameString + "\\";
                    GetDRVEntries(br, subdir);
                }
            }
            return entryList;
        }

        static void Main(string[] args)
        {


            if (args.Length == 0)
            {
                Console.WriteLine("Input file:");
                input = Console.ReadLine();
            }
            if (args.Length > 0)
                input = args[0];

            dir += (Path.GetFileName(input)) + "\\";


            using (FileStream fs = new FileStream(input, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (BinaryReader br = new BinaryReader(fs, new ASCIIEncoding()))
                {
                    GetDRVEntries(br, dir);
                }
            }
            Console.ReadLine();
        }
    }
}







