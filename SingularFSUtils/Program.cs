using SingularFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingularFS.Utils
{
	internal class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine(
					"SingularFS Utilities Help:\n" +
					"-E -f <SFSfile> <filename> <dir> - Exports a file to a directory\n" +
					"-E -d <SFSfile> <dir>            - Exports all files to a directory\n" +
					"-I -f <SFSfile> <path>           - Imports a file to a SingularFS archive\n" +
					"-I -d <SFSfile> <dir>            - Imports all files in a spesified directory to a SingularFS archive\n" +
					"-D <SFSfile> <filename>          - Deletes specified file\n" +
					"-L <SFSfile>                     - Lists all files in a SingularFS archive\n" +
					"-C <SFSfile>                     - Creates a SingularFS archive\n" +
					"<SFSfile>                        - Directory will be created with the content of SFSfile");
				return;
			}
			else if(args.Length == 1)
			{
				if (!args[0].EndsWith(".fs_\"") && !args[0].EndsWith(".fs_"))
				{
					return;
				}
				IFileSystem local = FSMod.CreateNew();
				try
				{
					local = FSMod.Import(args[0]);
				}
				catch (Exception)
				{
					return;
				}
				string dir = args[0].Split('\\').Last().Split('.').First();
				Directory.CreateDirectory(dir);
				foreach (string item in local.GetAllFiles())
				{
					File.WriteAllBytes(dir + "\\" + item, local.ReadAllBytes(item));
					Console.WriteLine(item);
				}
				return;
			}
			else if(args.Length == 2)
			{
				if (args[0] == "-L")
				{
					IFileSystem local = FSMod.Import(args[1]);
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Last Write Time     Size          Name");
					Console.WriteLine("-----------------   ------------- ----");
					//Console.WriteLine("19.02.2020  15:30   2,147,483,647 Nazwa Pliku.txt");
					Console.ResetColor();
					foreach (string item in local.GetAllFiles())
					{
						FileMetadata metadata = local.GetFileData(item);
						string size = metadata.Size.ToString("n0").PadLeft(13,' ');
						Console.WriteLine($"{metadata.CreationTime:dd.MM.yyyy  HH:mm}   {size} {metadata.Name}");
					}
					return;
				}
				if (args[0] == "-C")
				{
					IFileSystem local = FSMod.CreateNew();
					//local.WriteAllText("created@", DateTime.UtcNow.ToString("HH:mm:ss dd.MM.yyyy"));
					FSMod.Export(args[1], local);
					return;
				}
			}
			else if (args.Length == 3)
			{
				if (args[0] == "-D")
				{
					IFileSystem local = FSMod.Import(args[1]);
					local.Delete(args[2]);
					FSMod.Export(args[1],local);
				}
			}
			else if (args.Length == 4) 
			{
				if (args[0] == "-E" && args[1] == "-d")
				{
					IFileSystem local = FSMod.Import(args[2]);
					foreach (string item in local.GetAllFiles())
					{
						File.WriteAllBytes(args[3] + "\\" + item, local.ReadAllBytes(item));
						Console.WriteLine(item);
					}
					return;
				}
				else if (args[0] == "-I")
				{
					if (args[1] == "-f")
					{
						IFileSystem local = FSMod.Import(args[2]);
						string filename = args[3].Split('\\').Last();
						local.WriteAllBytes(filename, File.ReadAllBytes(args[3]));
						FSMod.Export(args[2], local);
						return;
					}
					if (args[1] == "-d")
					{
						IFileSystem local = FSMod.Import(args[2]);
						foreach (string item in Directory.GetFiles(args[3]))
						{
							string filename = item.Split('\\').Last();
							local.WriteAllBytes(filename, File.ReadAllBytes(item));
						}
						FSMod.Export(args[2], local);
						return;
					}
				}
			}
			else if (args.Length == 5)
			{
				if (args[0] == "-E" && args[1] == "-f")
				{
					IFileSystem local = FSMod.Import(args[2]);
					File.WriteAllBytes(args[4] + "\\" + args[3], local.ReadAllBytes(args[3]));
					Console.WriteLine(args[4] + "\\" + args[3]);
					return;
				}
			}
		}
	}
}
