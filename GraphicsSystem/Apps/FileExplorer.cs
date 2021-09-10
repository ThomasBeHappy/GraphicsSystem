using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using GraphicsSystem.Core;
using GraphicsSystem.Data;
using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Apps
{
    class FileExplorer : Window
    {
        static char[] name = new char[] { 'F', 'i', 'l', 'e', ' ', 'E','x','p','l','o','r','e','r','\0' };

        bool refresh = true;
        FontMono9x11 font = new FontMono9x11();

        List<DirectoryEntry> directoryEntries;
        public List<char[]> folderNames = new List<char[]>();

        public string path = @"ROOT";

        static Bitmap icon = FileSystemBitmaps.bitmap;

        public FileExplorer(uint x, uint y, uint width, uint height) : base(name, x, y, width, height, icon)
        {
        }

        public override void Update()
        {
            if (refresh)
            {
                if (path == "ROOT")
                {
                    folderNames.Clear();
                    directoryEntries = FileSystem.fs.GetVolumes();

                    foreach (var item in directoryEntries)
                    {
                        folderNames.Add(item.mName.ToCharArray());
                    }
                    refresh = false;
                }
            }

            Graphics.Rectangle(x, y, x + 60,y + height, Color.black);
            Graphics.Rectangle(x, y, x + width, y + 20, Color.black);

            //Draw all folders/volumes
            for (int i = 0; i < directoryEntries.Count; i++)
            {
                Graphics.DrawBitmapFromData((int)(x + 65), (int)(y + 35 + ((i * 50) + 10)), 50, 50, FileSystemBitmaps.bitmap, Color.black);
                Graphics.DrawString(x + 120, (uint)(y + 35 + ((i * 50) + 10)), font, folderNames[i], Color.white);
            }

        }
    }
}
