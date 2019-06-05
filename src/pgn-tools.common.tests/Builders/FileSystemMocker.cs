using Moq;
using pgn_tools.common;

namespace pgn
{
    public class FileSystemMocker
    {
        private readonly Mock<IFileSystemProvider> _mockFileSystem = new Mock<IFileSystemProvider>();

        public void VerifyGetFilesWasRecursive() => VerifyGetFiles(true, "*.pgn");

        public void VerifyGetFilesWasNotRecursive() => VerifyGetFiles(false, "*.pgn");

        public void SetupGetFiles(string[] strings, bool recurse = false, string pattern = "*.pgn") =>
            _mockFileSystem.Setup(fs => fs.GetFiles(It.IsAny<string>(), recurse, pattern))
                .Returns(strings);

        public IFileSystemProvider Object => _mockFileSystem.Object;
         
        public void SetupDirectoryExists(bool value) =>
            _mockFileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>()))
                .Returns(value);

        public void SetupFileExists(bool value) =>
            _mockFileSystem.Setup(fs => fs.FileExists(It.IsAny<string>()))
                .Returns(value);

        private void VerifyGetFiles(bool recurse, string pattern) =>
            _mockFileSystem.Verify(fs => fs.GetFiles(
                It.IsAny<string>(),
                recurse,
                "*.pgn"));
    }
}