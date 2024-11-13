using System;

namespace SingularFS.Version1FS
{
    internal struct FileData
    {
        public string FileName;
        public int Offset;
        public DateTime CreationTime;
        public byte[] Data;
    }
}
