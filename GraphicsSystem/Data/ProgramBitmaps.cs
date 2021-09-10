using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Data
{
    class ProgramBitmaps
    {
        [ManifestResourceStream(ResourceName = "GraphicsSystem.Data.Bitmaps.Clock.bmp")]
        static byte[] file;
        public static Bitmap bitmap = new Bitmap(file);
    }
}
