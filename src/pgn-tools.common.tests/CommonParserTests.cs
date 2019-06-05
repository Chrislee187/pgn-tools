using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using pgn;
using pgn_tools.common;

namespace Tests
{
    public class CommonParserTests
    {
        private static readonly string[] NoArgs = new string[0];
        private static readonly string[] OneArg = {"AnArg"};
        private static readonly string[] OneArgRecursive = {"AnArg", "--recurse"};
        private static readonly string[] TwoArgs = {"AnArg", "AnotherArg"};

        private CommonParser _parser;
        private readonly FileSystemMocker _fileSystemMocker = new FileSystemMocker();
        
        [SetUp]
        public void Setup()
        {
            _fileSystemMocker.SetupFileExists(true);
            _fileSystemMocker.SetupDirectoryExists(true);
            _fileSystemMocker.SetupGetFiles(new List<string>().ToArray());
        }

        [Test]
        public void No_arguments_sets_stdin_as_input_source()
        {
            _parser = new CommonParser(NoArgs, _fileSystemMocker.Object);
            Assert.That(_parser.HasErrors, Is.False);
            Assert.That(_parser.UseStdIn, Is.True);
            Assert.That(_parser.FileSources.Any(), Is.False);
        }

        [Test]
        public void Arguments_that_are_NOT_existing_files_or_directories_sets_error_state()
        {
            _fileSystemMocker.SetupFileExists(false);
            _fileSystemMocker.SetupDirectoryExists(false);
            _parser = new CommonParser(TwoArgs, _fileSystemMocker.Object);

            Assert.That(_parser.HasErrors, Is.True);
            Assert.That(_parser.Errors.OfType<FileNotFoundException>().Count(), Is.EqualTo(2));

            Assert.That(_parser.UseStdIn, Is.False);
            Assert.That(_parser.FileSources.Any(), Is.False);
        }
        
        [Test]
        public void Arguments_that_are_existing_files_adds_to_FileSources()
        {
            _fileSystemMocker.SetupFileExists(true);
            _parser = new CommonParser(TwoArgs, _fileSystemMocker.Object);

            Assert.That(_parser.HasErrors, Is.False);

            Assert.That(_parser.UseStdIn, Is.False);
            Assert.That(_parser.FileSources.Count(), Is.EqualTo(2));
            CollectionAssert.Contains(_parser.FileSources, TwoArgs[0]);
            CollectionAssert.Contains(_parser.FileSources, TwoArgs[1]);
        }


        [Test]
        public void Arguments_that_are_folders_are_expanded_to_FileSources()
        {
            _fileSystemMocker.SetupFileExists(false);
            _fileSystemMocker.SetupDirectoryExists(true);
            _fileSystemMocker.SetupGetFiles(new[] { "file1", "file2", "file3" });
            _parser = new CommonParser(OneArg, _fileSystemMocker.Object);

            Assert.That(_parser.HasErrors, Is.False);

            Assert.That(_parser.UseStdIn, Is.False);

            Assert.That(_parser.FileSources.Count(), Is.EqualTo(3));
            CollectionAssert.DoesNotContain(_parser.FileSources, OneArg.Single());
            
            _fileSystemMocker.VerifyGetFilesWasNotRecursive();
        }

        [Test]
        public void Recurse_flag_is_honoured()
        {
            _fileSystemMocker.SetupFileExists(false);
            _fileSystemMocker.SetupGetFiles(new[] { "file1", "file2", "file3" }, true);
            _parser = new CommonParser(OneArgRecursive, _fileSystemMocker.Object);

            Assert.That(_parser.HasErrors, Is.False);

            Assert.That(_parser.UseStdIn, Is.False);
            Assert.That(_parser.FileSources.Count(), Is.EqualTo(3));
            _fileSystemMocker.VerifyGetFilesWasRecursive();
        }
    }

}