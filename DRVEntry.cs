using System.IO;
using System.Text;

namespace WindowsFormsApplication1.Properties
{
    public class DRVEntry
    {
        public byte bitflag;
        public byte[] extension;
        public int offset;
        public int filesize;
        public int unk1;
        public byte[] filename;
        public byte[] padding;
        public string extensionString;
        public string fileNameString;
        public string fullfileName;

        public DRVEntry(BinaryReader br)
        {
            bitflag = br.ReadByte();
            extension = br.ReadBytes(3);
            offset = br.ReadInt32() * 0x800;
            filesize = br.ReadInt32();
            unk1 = br.ReadInt32();
            filename = br.ReadBytes(8);
            padding = br.ReadBytes(8);

            extensionString = Encoding.Default.GetString(extension);
            fileNameString = Encoding.Default.GetString(filename).Replace("\0", "");
            fullfileName = fileNameString + "." + extensionString;

        }

    }
}
