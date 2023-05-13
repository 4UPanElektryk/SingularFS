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
		public string Data;
		public List<HeaderData> files;
		public FS(string data, List<HeaderData> Headers)
		{
			Data = data;
			files = Headers;
		}
		public FS()
		{
			Data = "";
			files = new List<HeaderData>();
		}
		public bool Exists(string path)
		{
			foreach (HeaderData item in files)
			{
				if (item.FileName == path)
				{
					return true;
				}
			}
			return false;
		}
		public string ReadAllText(string path)
		{
			foreach (HeaderData item in files)
			{
				if (item.FileName == path)
				{
					return Data.Substring(item.StartIndex,item.Length);
				}
			}
			throw new FileNotFoundException();
		}
		public void Delete(string path)
		{
			for (int i = 0; i < files.Count; i++)
			{
				if (files[i].FileName == path)
				{
					int diff = 0 - files[i].Length;
					string preData = Data.Substring(0, files[i].StartIndex);
					string pastData = Data.Substring(files[i].StartIndex + files[i].Length);
					Data = preData + pastData;
					for (int j = i; j < files.Count(); j++)
					{
						HeaderData d = files[j];
						d.StartIndex -= diff;
						files[j] = d;
					}
					files.Remove(files[i]);
					return;
				}
			}
		}
		public void WriteAllText(string path, string content)
		{
			for (int i = 0; i < files.Count; i++)
			{
				if (files[i].FileName == path)
				{
					int diff = content.Length - files[i].Length;
					string preData = Data.Substring(0, files[i].StartIndex);
					string pastData = Data.Substring(files[i].StartIndex + files[i].Length);
					Data = preData + content + pastData;
					for (int j = i; j < files.Count(); j++)
					{
						HeaderData d = files[j];
						d.StartIndex -= diff;
						files[j] = d;
					}
					HeaderData dd = files[i];
					dd.Length -= content.Length;
					files[i] = dd;
					return;
				}
			}
			files.Add(new HeaderData() { FileName =path, Length = content.Length, StartIndex = Data.Length });
			Data += content;
		}
    }
}
