using System;
using System.IO;
using System.Text;
using SingularFS;

namespace SingularFS.Tests
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			FS local = new FS();
			if (File.Exists("plik.fs_"))
			{
				local = FSMod.Import("plik.fs_");
			}
			Random random = new Random();
			for (int i = 0; i < 800; i++)
			{
				local.WriteAllText(random.Next() + ".txt", Convert.ToBase64String(Encoding.UTF8.GetBytes(random.Next().ToString())));
			}
			FSMod.Export("plik.fs_",local);
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
