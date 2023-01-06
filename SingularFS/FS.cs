using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
			Path = path;
			if (File.Exists(path))
				files = Import();
			else
				files = new List<FSFile>();
		}
		public static void FileSave(FSFile file)
		{
			for (int i = 0; i < files.Count; i++)
			{
				if (files[i].Name == file.Name)
				{
					files[i] = file;
					return;
				}
			}
			files.Add(file);
		}
		public static bool Exists(string name)
		{

		}
		public static FSFile Load(string name)
		{
			foreach (FSFile item in files)
			{
				if (item.Name == name)
				{
					return item;
				}
			}
			return null;
		}
		public static List<FSFile> Import()
		{
			List<FSFile> Read = new List<FSFile>();
			string raw = File.ReadAllText(Path, Encoding.ASCII);
			string headers = raw.Split(char.ConvertFromUtf32(5).ToCharArray())[0];
			string data = raw.Substring(headers.Length);
			int currentindex = 0;
			string[] headersext = headers.Split(char.ConvertFromUtf32(4).ToCharArray());


			foreach (string item in headersext)
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
		public static void Export()
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
			
			char[] dataraw = data.ToCharArray().Skip(startindex).ToArray();
 			for (int i = 0; i < dataraw.Length; i++)
			{
				if (i == length)
				{
					return newstring;
				}
				newstring += dataraw[i];
			}
			return newstring;
		}
		public static void Encrypt(string key)
		{
			
		}
	}
}
