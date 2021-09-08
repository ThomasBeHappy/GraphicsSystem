using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Data
{
    public static class BackgroundBitmap
    {
        [ManifestResourceStream(ResourceName = "GraphicsSystem.Data.Gaming Frame.bmp")] 
        static byte[] file;
        public static Bitmap bitmap = new Bitmap(file);
    }
}
