﻿using System;
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

        public bool UseStdIn { get; }
        public string[] FileSources {get; private set; } = new List<string>().ToArray();
        public bool HasErrors => _errors.Any();
        public IEnumerable<Exception> Errors => _errors;

        public CommonParser(string[] args, IFileSystemProvider fileSystemProvider = null)
        {
            _fileSystemProvider = fileSystemProvider ?? new FileSystemProvider();
            SimpleParser = new SimpleParser(args);

            UseStdIn = !SimpleParser.Arguments.Any();

            if (!UseStdIn)
            {
                FileSources = ResolveFileSources(SimpleParser.Arguments, SimpleParser.HasFlag("recurse"));
            }
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
                    resolvedPaths.AddRange(_fileSystemProvider.GetFiles(fileSource, recurse, pattern));
                }
                //                else
                //                {
                //                    // TODO: Check for paths with patterns in
                //
                //                }
                else
                {
                    _errors.Add(new FileNotFoundException("Couldn't resolve file source.", fileSource));
                }
            }

            return resolvedPaths.ToArray();
        }

        class FileSystemProvider : IFileSystemProvider
        {
            public bool FileExists(string path) => File.Exists(path);
            public bool DirectoryExists(string path) => Directory.Exists(path);
            public string[] GetFiles(string path, bool recurse = false, string pattern = "*.pgn") => Directory.GetFiles(path);
        }
    }
}