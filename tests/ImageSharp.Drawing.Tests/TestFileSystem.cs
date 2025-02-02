// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.IO;

namespace SixLabors.ImageSharp.Drawing.Tests
{
    /// <summary>
    /// A test image file.
    /// </summary>
    public class TestFileSystem : IO.IFileSystem
    {
        private readonly Dictionary<string, Stream> fileSystem = new(StringComparer.OrdinalIgnoreCase);

        public void AddFile(string path, Stream data)
        {
            lock (this.fileSystem)
            {
                this.fileSystem.Add(path, data);
            }
        }

        public Stream Create(string path)
        {
            // if we have injected a fake file use it instead
            lock (this.fileSystem)
            {
                if (this.fileSystem.ContainsKey(path))
                {
                    Stream stream = this.fileSystem[path];
                    stream.Position = 0;
                    return stream;
                }
            }

            return File.Create(path);
        }

        public Stream OpenRead(string path)
        {
            // if we have injected a fake file use it instead
            lock (this.fileSystem)
            {
                if (this.fileSystem.ContainsKey(path))
                {
                    Stream stream = this.fileSystem[path];
                    stream.Position = 0;
                    return stream;
                }
            }

            return File.OpenRead(path);
        }
    }
}
