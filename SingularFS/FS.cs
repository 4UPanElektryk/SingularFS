using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SingularFS
{
	public class FS
	{
		public static string Path;
		public static List<FSFile> files;
		private static string EncryptionKey;
		public FS(string path)
		{
			files = new List<FSFile>();
			Path = path;

		}
		public static List<FSFile> Load()
		{
			List<FSFile> Read = new List<FSFile>();
			string raw = File.ReadAllText(Path, Encoding.UTF32);
			string[] file = raw.Split(char.ConvertFromUtf32(5).ToCharArray());
			string data = "";
			string[] metadata = new string[file.Length - 1];
			for (int i = 0; i < file.Length; i++)
			{
				if (i == file.Length - 1)
				{
					data = file[i];
				}
				else
				{
					metadata[i] = file[i];
				}
			}
			int currentindex = 0;
			foreach (string item in metadata)
			{
				string[] fdata = item.Split(char.ConvertFromUtf32(6).ToCharArray());
				string name = fdata[0];
				int length = int.Parse(fdata[1]);
				string datatobox = GetFormData(data,currentindex,length);
				currentindex += length;
				Read.Add(new FSFile(name,datatobox));
            }
			return Read;
		}
		public static void Save()
		{
			string end = "";
			foreach (FSFile item in files)
			{
				string output = item.Name + char.ConvertFromUtf32(6) + item.Length.ToString() + char.ConvertFromUtf32(5);
				end += output;
			}
            foreach (FSFile item in files)
			{
				end += item.Content; 
			}
			
			File.WriteAllText(Path,end,Encoding.UTF32);
		}
		private static string GetFormData(string data, int startindex, int length)
		{
			string newstring = "";
			char[] dataraw = data.ToCharArray();
 			for (int i = 0; i < dataraw.Length; i++)
			{
				if (i == startindex+length)
				{
					return newstring;
				}
				if (i >= startindex)
				{
					newstring += dataraw[i];
				}
			}
			return newstring;
		}
		public static void Encrypt(string key)
		{
			
		}
	}
}
