using System;
using System.IO;


namespace SingularFS
{
	public class FSMod
	{
		private static IFileSystem[] fileSystems =
		{
			new Version1FS.FS()
		};
		public static uint ChecksumFast(byte[] arr)
		{
			if (arr.Length == 0) return 0;

			uint sum0 = 0, sum1 = 0, sum2 = 0, sum3 = 0;

			for (var i = 0; i < arr.Length / 4; i+=4)
			{
				sum0 += arr[i];
				sum1 += arr[i+1];
				sum2 += arr[i+2];
				sum3 += arr[i+3];
			}
			for (var i = 0; i < arr.Length % 4 ; i++)
			{
				switch (i % 4)
				{
					case 0: sum0 += arr[i]; break;
					case 1: sum1 += arr[i]; break;
					case 2: sum2 += arr[i]; break;
					case 3: sum3 += arr[i]; break;
				}
			}
			var sum = sum3 + (sum2 << 8) + (sum1 << 16) + (sum0 << 24);
			return sum;
		}
		public static IFileSystem Import(string Path)
		{
			byte[] bytes = File.ReadAllBytes(Path);
			if (bytes.Length == 0) return null;
			if (bytes[0] == 0) throw new Exception("Not yet implemented or invalid file");
			foreach (IFileSystem item in fileSystems)
			{
				if (item.Version == bytes[0])
				{
					IExportable ready = Activator.CreateInstance(item.GetType()) as IExportable;
					ready.Import(bytes);
					return ready as IFileSystem;
				}
			}
			return null;
		}
		public static void Export(string Path, IFileSystem filesystem)
		{
			IExportable exportable = filesystem as IExportable;
			File.WriteAllBytes(Path,exportable.Export());
		}
		public static IFileSystem CreateNew(byte Version = 0)
		{
			if (Version == 0)
			{
				Version = (byte)(fileSystems.Length - 1);
			}
			return Activator.CreateInstance(fileSystems[Version].GetType()) as IFileSystem;
		}
	}
}
