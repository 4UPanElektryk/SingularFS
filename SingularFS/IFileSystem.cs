using SingularFS.Version1FS;
using System.Text;

namespace SingularFS
{
	public interface IFileSystem
	{
		byte Version { get; } // Version 0 is reserved for later use
		void Delete(string path);
		bool Exists(string path);
		string[] GetAllFiles();
		FileMetadata GetFileData(string path);
		byte[] ReadAllBytes(string path);
		void WriteAllBytes(string path, byte[] content);
		string ReadAllText(string path);
		string ReadAllText(string path, Encoding encoding);
		void WriteAllText(string path, string content);
		void WriteAllText(string path, string content, Encoding encoding);
	}
}
