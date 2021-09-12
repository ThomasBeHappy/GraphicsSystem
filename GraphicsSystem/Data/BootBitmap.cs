using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Data
{
    public static class BootBitmap
    {
        [ManifestResourceStream(ResourceName = "GraphicsSystem.Data.Bitmaps.Boot.bmp")]
        static byte[] file;
        public static Bitmap bitmap = new Bitmap(file);
    }
}
