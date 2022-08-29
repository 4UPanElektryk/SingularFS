using System;

namespace SingularFS
{
	public class FSFile
	{
		public string Name;
		public string Content;
		public int Length;
		public FSFile(string name,string content)
		{
			Name = name;
			Content = content;
			Length = content.Length;
		}
	}
}
