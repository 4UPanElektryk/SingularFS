using System;

namespace FileFS
{
    public class FileFSFile
    {
        public string Name;
        public string Content;
        public int Length;
        public static FileFSFile Create(string name,string[] content)
        {
            FileFSFile f = new FileFSFile
            {
                Name = name,
                Content = string.Join("\r\n", content),
                Length = string.Join("\r\n", content).Length,
            };
            return f;
        }
    }
}
