using System;
using System.IO;
using SingularFS;

namespace Test4FileFS
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			FS local = new FS();
			if (File.Exists("userdata.fs_"))
			{
				local = FSMod.Import("userdata.fs_");
			}
            string path = Console.ReadLine();
			local.WriteAllText(path, Console.ReadLine());
			FSMod.Export("userdata.fs_",local);
			foreach (HeaderData item in local.files)
			{
				Console.WriteLine();
				Console.WriteLine(item.FileName);
				Console.WriteLine(local.ReadAllText(item.FileName));
			}
			Console.ReadKey(true);
		}
	}
}
