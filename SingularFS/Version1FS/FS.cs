using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace SingularFS.Version1FS
{
    public class FS : IFileSystem, IExportable
	{
		private readonly byte Version = 1;
		internal List<FileData> files = new List<FileData>();
		byte IFileSystem.Version { get { return this.Version; } }

		public void Delete(string path)
		{
			if (!Exists(path)) { throw new FileNotFoundException("File Does not Exist", path); }
			FileData file = files.Find((x) => x.FileName == path);
			files.Remove(file);
		}
		public bool Exists(string path)
		{
			FileData file = files.Find((x) => x.FileName == path);
			return file.FileName == path;
		}
		public string[] GetAllFiles()
		{
			string[] strings = new string[files.Count];
			for (int i = 0; i < files.Count; i++)
			{
				strings[i] = files[i].FileName;
			}
			return strings;
		}
		public FileMetadata GetFileData(string path)
		{
			foreach (var item in files)
			{
				if (item.FileName == path)
				{
					return new FileMetadata() { CreationTime = item.CreationTime, Name = item.FileName, Offset = item.Offset, Size = item.Data.Length };
				}
			}
			throw new FileNotFoundException("File does not exist", path);
		}
		public string ReadAllText(string path)
		{
			return Encoding.UTF8.GetString(ReadAllBytes(path));
		}
		public string ReadAllText(string path, Encoding encoding)
		{
			return encoding.GetString(ReadAllBytes(path));
		}
		public void WriteAllText(string path, string content)
		{
			WriteAllBytes(path,Encoding.UTF8.GetBytes(content));
		}
		public void WriteAllText(string path, string content, Encoding encoding)
		{
			WriteAllBytes(path, encoding.GetBytes(content));
		}

		public byte[] ReadAllBytes(string path)
		{
			if (!Exists(path)) { throw new FileNotFoundException("File Does not Exist", path); }
			FileData file = files.Find((x) => x.FileName == path);
			return file.Data;
		}
		public void WriteAllBytes(string path, byte[] content)
		{
			if (Exists(path)) { Delete(path); }
			files.Add(new FileData() { CreationTime = DateTime.UtcNow, Data = content, FileName = path});
		}

		public void Import(byte[] bytes)
		{
			Header header = new Header
			{
				Version = bytes[0],
				checksum = BitConverter.ToUInt32(bytes, 1),
				CreationTime = DateTime.FromBinary(BitConverter.ToInt64(bytes, 5)),
				FileHeaderLength = BitConverter.ToInt32(bytes, 13),
			};
			byte[] bytesFileHeaders = bytes.Skip(17).Take(header.FileHeaderLength).ToArray();
			byte[] bytesFileData = bytes.Skip(header.FileHeaderLength + 17).ToArray();
			files = CalcFileFromTableEntry(bytesFileHeaders);
			for (int i = 0; i < files.Count - 1; i++)
			{
				FileData data = files[i];
				data.Data = bytesFileData.Skip(data.Offset).Take(files[i+1].Offset - data.Offset).ToArray();
				files[i] = data;
			}
			FileData d = files[files.Count - 1];
			d.Data = bytesFileData.Skip(d.Offset).ToArray();
			files[files.Count - 1] = d;
		}
		public byte[] Export()
		{
			Header header = new Header()
			{
				Version = Version,
				CreationTime = DateTime.UtcNow
			};
			int totalLength = 0;
			foreach (var file in files) 
			{ 
				totalLength += file.Data.Length;
			}
			byte[] FilesBytesArr = new byte[totalLength];
			int i = 0;
			foreach (var file in files)
			{
				for (int j = 0; j < file.Data.Length; j++)
				{
					FilesBytesArr[i + j] = file.Data[j]; 
				}
				i += file.Data.Length;
			}
			header.checksum = FSMod.ChecksumFast(FilesBytesArr);
			List<byte> bytes = new List<byte>();
			
			#region File Table Calculation
			List<byte> bytesoffiletable = new List<byte>();
			int startoffset = 0;
			int totalfileheadersize = 0;
			for (int j = 0; j < files.Count; j++)
			{
				FileData d = files[j];
				d.Offset = startoffset;
				startoffset += d.Data.Length;
				files[j] = d;
				byte[] arr = CalcFileTableEntry(d);
				totalfileheadersize += arr.Length;
				bytesoffiletable.AddRange(arr);
			}
			header.FileHeaderLength = totalfileheadersize;
			#endregion
			
			#region Archive Header
			bytes.Add(header.Version);
			bytes.AddRange(BitConverter.GetBytes(header.checksum));
			bytes.AddRange(BitConverter.GetBytes(header.CreationTime.ToBinary()));
			bytes.AddRange(BitConverter.GetBytes(header.FileHeaderLength));
			#endregion

			bytes.AddRange(bytesoffiletable);

			#region File Bytes
			bytes.AddRange(FilesBytesArr);
			#endregion
			return bytes.ToArray();
		}
		private List<FileData> CalcFileFromTableEntry(byte[] bytes) 
		{
			if (bytes.Length == 0)
			{
				return new List<FileData>();
			}
			FileData f = new FileData();
			byte[] name = bytes.Skip(1).Take(bytes[0]).ToArray();
			f.FileName = Encoding.ASCII.GetString(name);
			f.CreationTime = DateTime.FromBinary(BitConverter.ToInt64(bytes,bytes[0] + 1));
			f.Offset = BitConverter.ToInt32(bytes, bytes[0] + 9);
			List<FileData> files = new List<FileData> { f };
			files.AddRange(CalcFileFromTableEntry(bytes.Skip(bytes[0]+13).ToArray()));
			return files;
		}
		private byte[] CalcFileTableEntry(FileData file)
		{
			List<byte> bytes = new List<byte>(1 + file.FileName.Length + 4);
			bytes.Add((byte)file.FileName.Length);
			bytes.AddRange(Encoding.ASCII.GetBytes(file.FileName));
			bytes.AddRange(BitConverter.GetBytes(file.CreationTime.ToBinary()));
			bytes.AddRange(BitConverter.GetBytes(file.Offset));
			return bytes.ToArray();
		}
	}
}
