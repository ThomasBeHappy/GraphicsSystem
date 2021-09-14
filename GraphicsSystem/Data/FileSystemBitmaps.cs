using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Data
{
    class FileSystemBitmaps
    {
        [ManifestResourceStream(ResourceName = "GraphicsSystem.Data.Bitmaps.Folder.bmp")]
        static byte[] file;
        public static Bitmap bitmap = new Bitmap(file);

        [ManifestResourceStream(ResourceName = "GraphicsSystem.Data.Bitmaps.File.bmp")]
        static byte[] file2;
        public static Bitmap fileBitmap = new Bitmap(file2);
    }
}
