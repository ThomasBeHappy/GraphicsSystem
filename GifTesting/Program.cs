using System;
using System.IO;
using GifParser;

namespace GifTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytes = File.ReadAllBytes("polyfish_lines.gif");

            GifParser.GifParser.Parse(bytes);

            Console.ReadLine();
        }
    }
}
