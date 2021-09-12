using System;
using System.Collections.Generic;

namespace GifParser
{
    public class Gif
    {
        public int speed;
        public Bitmap[] bitmaps;
        public int gifVersion;
        public int minScreenWidth, minScreenHeight;
    }

    public struct Bitmap
    {
        public uint[] imageData;
        public uint width;
        public uint height;

        public bool interlaced;

        public int x;
        public int y;
    }

    public struct ColorTableEntry
    {
        public uint color;
        public ColorTableEntry(byte r, byte g, byte b)
        {
            color = 255 * 255 * (uint)r + 255 * (uint)g + b;
        }
    }

    public static class GifParser
    {
        public static Gif Parse(byte[] data)
        {
            Gif gif = new Gif();

            #region Header
            if (data[0] != 'G' || data[1] != 'I' || data[2] != 'F')
            {
                throw new Exception("Invalid Gif Format");
            }

            if (data[3] == '8' && data[4] == '7' && data[5] == 'a')
            {
                gif.gifVersion = 1;
            }else
            {
                gif.gifVersion = 2;
            }
            #endregion

            gif.minScreenWidth = BitConverter.ToUInt16(data, 6);
            gif.minScreenHeight = BitConverter.ToUInt16(data, 8);

            #region Logical Screen Descriptor
            int sizeGlobalColorTable = 1 << ((data[10] & 0b111) + 1);
            bool colorTableSortFlag = (data[10] & 0b1000) == 0b1000;
            int colorResolution = data[10] & 0b1110000 >> 4;
            bool globalColorTableFlag = (data[10] & 0b10000000) == 0b10000000;
            uint backgroundColor = data[11];
            int aspectRatio = 1;
            #endregion

            int pos = 13;

            #region Global Color Table
            ColorTableEntry[] colorTable = new ColorTableEntry[sizeGlobalColorTable];
            if (globalColorTableFlag)
            {
                for (int i = 0; i < colorTable.Length; i++)
                {
                    colorTable[i] = new ColorTableEntry(data[pos++], data[pos++], data[pos++]);
                }
            }
            #endregion

            #region Images
            List<Bitmap> images = new List<Bitmap>();
            while (data[pos] != 0x3b)
            {
                Console.WriteLine(data[pos]);
                if (data[pos] != 0x2c)
                {
                    throw new Exception("Invalid Image Data");
                }
                pos++;
                Bitmap image = new Bitmap();
                image.x = BitConverter.ToUInt16(data, pos);
                pos+=2;
                image.y = BitConverter.ToUInt16(data, pos);
                pos += 2;
                image.width = BitConverter.ToUInt16(data, pos);
                pos += 2;
                image.height = BitConverter.ToUInt16(data, pos);
                pos += 2;
                image.imageData = new uint[image.width * image.height];
                bool localColorTableFlag = (data[pos] & 0b1) == 0b1;
                image.interlaced = (data[pos] & 0b10) == 0b10;
                bool sort = (data[pos] & 0b100) == 0b100;
                int localColorTableSize = 1 << ((data[pos] & 0b11100000 >> 5) + 1);

                pos++;

                ColorTableEntry[] localColorTable = new ColorTableEntry[localColorTableSize];
                if (localColorTableFlag)
                {
                    for (int i = 0; i < localColorTableSize; i++)
                    {
                        localColorTable[i] = new ColorTableEntry(data[pos++], data[pos++], data[pos++]);
                    }
                }

                byte[] subBlock = new byte[255];
                int compressPos = 0;
                while (data[pos] != 0)
                {
                    byte length = data[pos++];
                    subBlock = new byte[length];
                    for (int i = 0; i < length; i++)
                    {
                        subBlock[i] = data[pos++];
                    }

                    byte[] decompressed = LZWDecoder.Decode(subBlock);

                    if (localColorTableFlag)
                    {
                        for (int i = 0; i < decompressed.Length; i++)
                        {
                            image.imageData[compressPos] = localColorTable[decompressed[i]].color;
                            compressPos++;
                        }
                    }else
                    {
                        for (int i = 0; i < decompressed.Length; i++)
                        {
                            image.imageData[compressPos] = colorTable[decompressed[i]].color;

                        }
                    }
                }
                images.Add(image);

            }
            #endregion

            return null;
        }
    }
}
