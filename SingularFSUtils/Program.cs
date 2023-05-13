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
                Console.WriteLine("SingularFS Utilities Help:\n" +
					"-E -f <SFSfile> <filename> <dir> - Exports a files to a directory\n" +
					"-E -d <SFSfile> <dir>            - Exports all files to a directory\n" +
					"-I -f <SFSfile> <path>           - Imports a file to a SingularFS archive\n" +
					"-I -d <SFSfile> <dir>            - Imports all files in a spesified directory to a SingularFS archive\n" +
					"-D <SFSfile> <filename>          - Deletes specified file\n" +
					"-L <SFSfile>                     - Lists all files in a SingularFS archive\n" +
					"<SFSfile>                        - Directory will be created with the content of SFSfile");
				return;
            }
			else if(args.Length == 1)
			{
				if (!args[0].EndsWith(".fs_\"") && !args[0].EndsWith(".fs_"))
				{
					return;
				}
				FS local = new FS();
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
				foreach (HeaderData item in local.files)
				{
					File.WriteAllText(dir + "\\" + item.FileName, local.ReadAllText(item.FileName));
					Console.WriteLine(item.FileName);
				}
				return;
			}
			else if(args.Length == 2)
			{
                if (args[0] == "-L")
				{
                    FS local = FSMod.Import(args[1]);
					foreach (HeaderData item in local.files)
					{
						Console.WriteLine(item.FileName);
					}
					return;
				}
			}
			else if (args.Length == 3)
			{
				if (args[0] == "-D")
				{
					FS local = FSMod.Import(args[1]);
					local.Delete(args[2]);
					FSMod.Export(args[1],local);
				}
			}
			else if (args.Length == 4) 
			{
				if (args[0] == "-E" && args[1] == "-d")
				{
					FS local = FSMod.Import(args[2]);
					foreach (HeaderData item in local.files)
					{
						File.WriteAllText(args[3] + "\\" + item.FileName, local.ReadAllText(item.FileName));
						Console.WriteLine(item.FileName);
					}
					return;
				}
				else if (args[0] == "-I")
				{
					if (args[1] == "-f")
					{
						FS local = FSMod.Import(args[2]);
						string filename = args[3].Split('\\').Last();
						local.WriteAllText(filename, File.ReadAllText(args[3]));
						FSMod.Export(args[2], local);
						return;
					}
					if (args[1] == "-d")
					{
						FS local = FSMod.Import(args[2]);
						foreach (string item in Directory.GetFiles(args[3]))
						{
							string filename = item.Split('\\').Last();
							local.WriteAllText(filename, File.ReadAllText(item));
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
					FS local = FSMod.Import(args[2]);
					File.WriteAllText(args[4] +"\\"+ args[3], local.ReadAllText(args[3]));
                    Console.WriteLine(args[4] + "\\" + args[3]);
                    return;
				}
			}
		}
	}
}
