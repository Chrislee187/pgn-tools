using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YACLAP;

namespace pgn_tools.common
{
    public class CommonParser
    {
        protected readonly SimpleParser SimpleParser;
        private List<Exception> _errors = new List<Exception>();
        private IFileSystemProvider _fileSystemProvider;

        public string[] FileSources {get; private set; } = new List<string>().ToArray();
        public bool HasErrors => _errors.Any();
        public IEnumerable<Exception> Errors => _errors;
        public bool Debug => SimpleParser.HasFlag("debug");

        public CommonParser(string[] args, IFileSystemProvider fileSystemProvider = null)
        {
            _fileSystemProvider = fileSystemProvider ?? new FileSystemProvider();
            SimpleParser = new SimpleParser(args);

            FileSources = ResolveFileSources(SimpleParser.Arguments, SimpleParser.HasFlag("recurse"));
        }

        private string[] ResolveFileSources(string[] sources, bool recurse = false, string pattern = "*.pgn")
        {
            var resolvedPaths = new List<string>();
            foreach (var fileSource in sources)
            {
                if (_fileSystemProvider.FileExists(fileSource))
                {
                    resolvedPaths.Add(fileSource);
                }
                else if (_fileSystemProvider.DirectoryExists(fileSource))
                {
                    resolvedPaths.AddRange(_fileSystemProvider.GetFiles(fileSource, pattern, recurse));
                }
                //                else
                //                {
                //                    // TODO: Check for paths with patterns in
                //                }
                else
                {
                    _errors.Add(new FileNotFoundException($"Couldn't resolve file source: {fileSource}", fileSource));
                }
            }

            return resolvedPaths.ToArray();
        }

        // NOTE: There are no tests for this class as is only a simple pass through to system library calls
        class FileSystemProvider : IFileSystemProvider
        {
            public bool FileExists(string path) => File.Exists(path);
            public bool DirectoryExists(string path) => Directory.Exists(path);
            public string[] GetFiles(string path, string pattern = "*.pgn", bool recurse = false) 
                => Directory.GetFiles(
                    path, 
                    pattern, 
                    recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }
    }
}