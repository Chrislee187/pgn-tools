namespace pgn_tools.common
{
    public interface IFileSystemProvider
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);
        string[] GetFiles(string path, bool recurse = false, string pattern = "*.pgn");
    }
}