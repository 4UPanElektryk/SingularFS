using System;
using System.IO;
using SingularFS;

namespace Test4FileFS
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			new FS("data.sfsa");
			if (!File.Exists("data.sfsa"))
			{
				FS.Save();
			}
			FS.files = FS.Load();
            string path = Console.ReadLine();
            FS.files.Add(new FSFile(path, File.ReadAllText(path)));
			FS.Save();
			Console.ReadLine();
			foreach (FSFile item in FS.Load())
			{
				File.WriteAllText(item.Name, item.Content);
				Console.WriteLine(item.Name);
				Console.WriteLine(item.Content);
				Console.WriteLine();
			}
			Console.ReadLine();
		}
	}
}
