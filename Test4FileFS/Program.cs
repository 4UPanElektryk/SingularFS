using SingularFS.Version1FS;
using System;
using System.IO;
using System.Text;

namespace SingularFS.Tests
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			IFileSystem local = FSMod.CreateNew();
			if (File.Exists("plik.fs_"))
			{
				local = FSMod.Import("plik.fs_");
				foreach (var item in local.GetAllFiles())
				{
					FileData file = local.GetFileData(item);
                    Console.WriteLine($"{file.FileName} {file.Offset} {file.CreationTime}");
				}
				Console.ReadKey(true);
				return;
			}
			Random random = new Random();
			for (int i = 0; i < 20; i++)
			{
				local.WriteAllText(random.Next() + ".txt", Convert.ToBase64String(Encoding.UTF8.GetBytes(random.Next().ToString())));
			}
			FSMod.Export("plik.fs_",local);
			foreach (string item in local.GetAllFiles())
			{
				Console.WriteLine();
				Console.WriteLine(item);
				Console.WriteLine(local.ReadAllText(item));
			}
			Console.ReadKey(true);
		}
	}
}
