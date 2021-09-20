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

            try
            {
                //fs.GetDirectoryListing(@"0:\");
                if (!Directory.Exists(@"0:\System"))
                {
                    Directory.CreateDirectory(@"0:\System");
                    Directory.CreateDirectory(@"0:\System\SystemApps");
                    Directory.CreateDirectory(@"0:\System\Audio");
                    Directory.CreateDirectory(@"0:\System\Images");
                    Directory.CreateDirectory(@"0:\System\Images\Bitmaps");
                    Directory.CreateDirectory(@"0:\System\Images\Gifs");
                    Directory.CreateDirectory(@"0:\System\Logs");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("WARNING: Could not access drive 0!");
            }
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

        public static void Format(string drive)
        {
            fs.Format(drive, "FAT32", true);
        }
    }
}
