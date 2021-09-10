using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.FileSystem.VFS;

namespace GraphicsSystem.Core
{
    public static class FileSystem
    {
        public static CosmosVFS fs;

        public static void Initialize()
        {
            fs = new CosmosVFS();

            VFSManager.RegisterVFS(fs);

            List<DirectoryEntry> volumes = fs.GetVolumes();

            foreach (var item in volumes)
            {
                System.Console.WriteLine(item.mFullPath);
            }

            //if (fs.GetFileSystemType(@"0:\") != "FAT32")
            //{
            //    fs.Format("0", "FAT32", true);
            //}
        }

        public static byte[] ReadSmallFile(string path)
        {
            return File.ReadAllBytes(path);
        }

        //public static byte[] ReadLargeFile(string path, int fileSize)
        //{
        //    Stream stream = fs.GetFile(path).GetFileStream();

        //    stream.
        //}
    }
}
