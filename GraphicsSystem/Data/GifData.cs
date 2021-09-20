using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Text;
using GifParser;

namespace GraphicsSystem.Data
{
    public static class GifData
    {

        [ManifestResourceStream(ResourceName = "GraphicsSystem.Data.Gifs.test.gif")]
        static byte[] file;
        public static Gif gif = GifParser.GifParser.Parse(file);
    }
}
