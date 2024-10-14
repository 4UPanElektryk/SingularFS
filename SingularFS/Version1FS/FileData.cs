using System;
using System.Collections.Generic;
using System.Text;

namespace SingularFS.Version1FS
{
    public struct FileData
    {
        public string FileName;
        public int Offset;
        public DateTime CreationTime;
        public byte[] Data;
    }
}
