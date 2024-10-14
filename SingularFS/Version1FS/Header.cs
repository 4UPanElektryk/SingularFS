using System;
using System.Collections.Generic;
using System.Text;

namespace SingularFS.Version1FS
{
    public struct Header
    {
        public byte Version;
        public uint checksum;
        public DateTime CreationTime;
        public int FileHeaderLength;
    }
}
