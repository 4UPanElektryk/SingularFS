using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace SingularFS
{
	public class FSMod
	{
		public static FS Import(string Path)
		{
			string raw = File.ReadAllText(Path, Encoding.ASCII);
			string headersraw = raw.Split(Convert.ToChar(1))[0];
			List<HeaderData> headers = new List<HeaderData>();
			int i = 0;
			HeaderData l = new HeaderData();
			foreach (var item in headersraw.Split(Convert.ToChar(0)))
			{
				if (i == 0)
				{
					l.FileName = item;
					i++;
				}
				else if(i == 1)
				{
					l.StartIndex = int.Parse(item);
					i++;
				}
				else if (i == 2)
				{
					l.Length = int.Parse(item);
					headers.Add(l);
					i = 0;
				}
			}
			string data = raw.Substring(headersraw.Length + 1);
			return new FS(data,headers);
		}
		public static void Export(string Path, FS filesystem)
		{
			string raw = "";
			foreach (HeaderData item in filesystem.files)
			{
				raw += item.FileName + Convert.ToChar(0) + item.StartIndex + Convert.ToChar(0) + item.Length + Convert.ToChar(0);
			}
			var temp = raw.ToCharArray(); 
			temp[raw.Length - 1] = Convert.ToChar(1);
			raw = new string(temp);
			raw += filesystem.Data;
			File.WriteAllText(Path, raw);
		}
	}
}
