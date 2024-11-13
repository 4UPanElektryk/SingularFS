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
			}
			Random random = new Random();
			for (int i = 0; i < 20; i++)
			{
				local.WriteAllText(random.Next() + ".txt", Convert.ToBase64String(Encoding.UTF8.GetBytes(random.Next().ToString())));
			}
			FSMod.Export("plik.fs_",local);
			foreach (var item in local.GetAllFiles())
			{
				FileMetadata file = local.GetFileData(item);
				Console.WriteLine($"{file.FileName} {file.Offset} {file.CreationTime}");
				Console.WriteLine(local.ReadAllText(item));
			}
			Console.ReadKey(true);
		}
	}
}
