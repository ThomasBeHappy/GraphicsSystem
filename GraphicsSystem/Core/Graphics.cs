#define COSMOSDEBUG
using Cosmos.Core;
using Cosmos.Debug.Kernel;
using Cosmos.HAL;
using Cosmos.HAL.Drivers.PCI.Video_plug;
using Cosmos.System.Graphics;
using GraphicsSystem.Hardware;
using GraphicsSystem.Types;
using Cosmos.System.Graphics.Fonts;
using System.Collections.Generic;
using Point = GraphicsSystem.Types.Point;
using Sys = Cosmos.System;
using System;

namespace GraphicsSystem.Core
{

    public struct Chunk
    {
        public uint startX, startY, endX, endY;
        public bool bufferChanged;

        public int width, height;
    }

    public static class Graphics
    {
        public static VMWareSVGAII driver;

        public const int width = 1920, height = 1080;
        public static int FONT_SPACING = 1;
        public static uint[] buffer;
        private static uint[] oldBuffer;

        public static Chunk[] chunks = new Chunk[6];

        private static int frames = 0;
        public static int fps { get; private set; } = 0;
        public static float delta { get; private set; } = 0;
        private static int tick = 0;

        public static void Initialize()
        {
            driver = new VMWareSVGAII();
            driver.SetMode(width, height);
            buffer = new uint[width * height];
            oldBuffer = new uint[width * height];
            //_debugger.Send(buffer.Length.ToString());
            Sys.MouseManager.ScreenWidth = width;
            Sys.MouseManager.ScreenHeight = height;
            ClearBuffer(Color.gray160);
            //int chunksX = 8 / 2;
            //int chunksY = 8 / 4;

            //uint chunksWidth = width / 3;
            //uint chunksHeight = height / 2;
            //int index = 0;
            //for (uint i = 0; i < 3; i++)
            //{
            //    for (uint j = 0; j < 2; j++)
            //    {
            //        chunks[index].endX = (chunks[index].startX = chunksWidth * i) + chunksWidth;
            //        chunks[index].endY = (chunks[index].startY = chunksHeight * j) + chunksHeight;

            //        chunks[index].bufferChanged = false;
            //        chunks[index].width = (int)chunksWidth;
            //        chunks[index].height = (int)chunksHeight;

            //        index++;
            //    }
            //}

            //chunks = GetChunkGrid(9, 16, width, height);

        }

        // TODO use a chunk system, split screen into 6 chunks
        public static void Update()
        {
            driver.Video_Memory.Copy(buffer);
            driver.Update(0, 0, width, height);

            ClearBuffer(Color.gray160);

            if (frames > 0) { delta = (float)1000 / (float)frames; }
            int sec = RTC.Second;
            if (tick != sec)
            {
                fps = frames;
                frames = 0;
                tick = sec;
            }
            frames++;
        }

        public unsafe static void ClearBuffer(uint color = 0)
        {
            fixed(uint* bufferPtr = &buffer[0]){
                MemoryOperations.Fill(bufferPtr, color, width * height);
            }
        }

        public static void UpdateCursor()
        {
            Point position = Mouse.position;
            if (position != Mouse.positionOld)
            {
                for (int x = 0; x < 12; x++)
                {
                    for (int y = 0; y < 20; y++)
                    {
                        if (Cursor.arrow[x + y * 12] != Color.gray160)
                        {
                            SetPixel((uint)(position.x + x), (uint)(position.y + y), Cursor.arrow[x + y * 12]);
                        }
                    }
                }
                Mouse.positionOld = position;
            }
        }

        #region EditBufferMethods

        static int x0, x1, x2, x3, x4, x5, x6, y0, y1, deltax, deltay, error, ystep, y;
        static bool steep;
        public static void DrawLine(int aX, int aY, int endX, int endY, uint color)
        {
            x0 = aX;
            x1 = endX;
            y0 = aY;
            y1 = endY;
            steep = System.Math.Abs(y1 - y0) > System.Math.Abs(x1 - x0);
            if (steep)
            {
                x3 = y0;
                y0 = x0;
                x0 = x3;
                x4 = y1;
                y1 = x1;
                x1 = x4;
            }
            if (x0 > x1)
            {
                x5 = x0;
                x0 = x1;
                x1 = x5;
                x6 = y0;
                y0 = y1;
                y1 = x6;
            }
            deltax = x1 - x0;
            deltay = System.Math.Abs(y1 - y0);
            error = deltax / 2;
            y = y0;
            if (y0 < y1)
            {
                ystep = 1;
            }
            else
            {
                ystep = -1;
            }
            for (int x = x0; x <= x1; x++)
            {
                if (steep)
                {
                    SetPixel((uint)y, (uint)x, color);
                }
                else
                {
                    SetPixel((uint)x, (uint)y, color);
                }
                error = error - deltay;
                if (error < 0)
                {
                    y = y + ystep;
                    error = error + deltax;
                }
            }

        }

        //static int[] sine = new int[16] { 0, 27, 54, 79, 104, 128, 150, 171, 190, 201, 221, 233, 243, 250, 254, 255 };
        //static int xEnd, yEnd, quadrant, x_flip, y_flip;

        static double angleX, angleY;

        public static void DrawAngle(int X, int Y, int angle, int radius, uint color)
        {
            angleY = radius * System.Math.Cos(System.Math.PI * 2 * angle / 360);
            angleX = radius * System.Math.Sin(System.Math.PI * 2 * angle / 360);

            DrawLine(X, Y, X + (int)(System.Math.Round(angleX * 100)/100), Y - (int)(System.Math.Round(angleY * 100) / 100), color);
        }

        public static void SetPixel(uint x, uint y, uint color)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                if (x + y * width > width * height)
                {
                    throw new Exception("Tried setting a pixel outside of the screen width and height");
                }else
                {
                    if (buffer[x + y * width] != color)
                    {
                        buffer[x + y * width] = color;
                    }
                }
            }
        }

        public unsafe static void Rectangle(uint x, uint y, uint endX, uint endY, uint color, bool border = false, uint borderColor = 0, uint borderThickness = 0)
        {
            if (x <= 0 && x > width && y <= 0 && y > height) return;

            if (border)
            {
                uint _width = endX - x;
                uint _height = endY - y;

                for (int i = 0; i < _width; i++)
                {
                    for (int h = 0; h < _height; h++)
                    {
                        if (h < borderThickness || _height - h <= borderThickness)
                        {
                            buffer[(x + i) + (y + h) * width] = borderColor;
                        }
                        else if (_width - i <= borderThickness || i < borderThickness)
                        {
                            buffer[(x + i) + (y + h) * width] = borderColor;
                        }
                        else
                        {
                            buffer[(x + i) + (y + h) * width] = color;
                        }
                    }
                }
            }
            else
            {
                int _width = (int)(endX - x);
                int _height = (int)(endY - y);

                fixed (uint* bufferPtr = &buffer[0])
                {
                    for (int i = 0; i < _height; i++)
                    {
                        MemoryOperations.Fill(bufferPtr + x + (y + i) * width, color, _width);
                    }
                }
            }
        }

        public static uint GetPixel(uint x, uint y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                return buffer[x + y * width];
            }
            return Color.black;

        }

        public static void DrawImage(Image image, int x, int y)
        {
            if (image.Width > width || image.Height > height) return;
            if (x <= 0 && x > width && y <= 0 && y > height) return;


            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    buffer[(x + w) * (y + h) * width] = (uint)image.rawData[w + h * image.Width];
                }
            }
        }

        public static void DrawCircle(uint x, uint y, uint radius, uint color, bool border = false, uint borderColor = 0, uint borderThickness = 0)
        {
            if (x <= 0 && x > width && y <= 0 && y > height) return;

            if (border)
            {
                int _x = (int)radius;
                int _y = 0;
                int xChange = (int)(1 - (radius << 1));
                int yChange = 0;
                int radiusError = 0;

                while (_x >= _y)
                {
                    for (int i = (int)(x - _x); i <= x + _x; i++)
                    {
                        SetPixel((uint)i, (uint)(y + _y), borderColor);
                        SetPixel((uint)i, (uint)(y - _y), borderColor);
                    }
                    for (int i = (int)(x - _y); i <= x + _y; i++)
                    {
                        SetPixel((uint)i, (uint)(y + _x), borderColor);
                        SetPixel((uint)i, (uint)(y - _x), borderColor);
                    }

                    _y++;
                    radiusError += yChange;
                    yChange += 2;
                    if (((radiusError << 1) + xChange) > 0)
                    {
                        _x--;
                        radiusError += xChange;
                        xChange += 2;
                    }
                }

                _x = (int)(radius - borderThickness / 2);
                _y = 0;
                xChange = (int)(1 - (radius << 1));
                yChange = 0;
                radiusError = 0;

                while (_x >= _y)
                {
                    for (int i = (int)(x - _x); i <= x + _x; i++)
                    {

                        SetPixel((uint)i, (uint)(y + _y), color);
                        SetPixel((uint)i, (uint)(y - _y), color);
                    }
                    for (int i = (int)(x - _y); i <= x + _y; i++)
                    {
                        SetPixel((uint)i, (uint)(y + _x), color);
                        SetPixel((uint)i, (uint)(y - _x), color);
                    }

                    _y++;
                    radiusError += yChange;
                    yChange += 2;
                    if (((radiusError << 1) + xChange) > 0)
                    {
                        _x--;
                        radiusError += xChange;
                        xChange += 2;
                    }
                }
            }
            else
            {
                int _x = (int)radius;
                int _y = 0;
                int xChange = (int)(1 - (radius << 1));
                int yChange = 0;
                int radiusError = 0;

                while (_x >= _y)
                {
                    for (int i = (int)(x - _x); i <= x + _x; i++)
                    {

                        SetPixel((uint)i, (uint)(y + _y), color);
                        SetPixel((uint)i, (uint)(y - _y), color);
                    }
                    for (int i = (int)(x - _y); i <= x + _y; i++)
                    {
                        SetPixel((uint)i, (uint)(y + _x), color);
                        SetPixel((uint)i, (uint)(y - _x), color);
                    }

                    _y++;
                    radiusError += yChange;
                    yChange += 2;
                    if (((radiusError << 1) + xChange) > 0)
                    {
                        _x--;
                        radiusError += xChange;
                        xChange += 2;
                    }
                }
            }
        }

        public static void DrawCharNew(int x, int y, char c, uint color, Sys.Graphics.Fonts.Font font)
        {
            int p = font.Height * (byte)c;

            for (int cy = 0; cy < font.Height; cy++)
            {
                for (byte cx = 0; cx < font.Width; cx++)
                {
                    if (font.ConvertByteToBitAddres(font.Data[p + cy], cx + 1))
                    {
                        SetPixel((ushort)(x + (font.Width - cx)), (ushort)(y + cy), color);
                    }
                }
            }

        }


        public static void DrawBitmap(int x, int y, int width, int height, uint color, uint[] data)
        {
            for (int i = 0; i < width * height; i++)
            {
                int xx = x + (i % width);
                int yy = y + (i / width);

                if (data[i] != 0)
                {
                    SetPixel((uint)xx, (uint)yy, color);
                }
            }
        }

        public static unsafe void DrawBitmapFromData(int aX, int aY, int aWidth, int aHeight, Bitmap data)
        {
            if (aWidth + aX > width)
            {
                aWidth -= (width - (aWidth + aX));
            }
            if (aHeight + aY > height)
            {
                aHeight -= (height - (aHeight + aY));
            }

            fixed (uint* bufferPtr = &buffer[0]){
                fixed(int* falseImgPtr = &data.rawData[0]){
                    uint* imgPtr = (uint*)falseImgPtr;
                    for (int y = 0; y < aHeight; y++)
                    {
                        MemoryOperations.Copy(bufferPtr + aX + (aY + y) * width, imgPtr + y * aWidth, aWidth);
                    }
                }
            }
        }

        public static unsafe void DrawBitmapFromData(int aX, int aY, int aWidth, int aHeight, uint[] data)
        {
            if (aWidth + aX > width)
            {
                aWidth -= (width - (aWidth + aX));
            }
            if (aHeight + aY > height)
            {
                aHeight -= (height - (aHeight + aY));
            }

            fixed (uint* bufferPtr = &buffer[0])
            {
                fixed (uint* imgPtr = &data[0])
                {
                    for (int y = 0; y < aHeight; y++)
                    {
                        MemoryOperations.Copy(bufferPtr + aX + (aY + y) * width, imgPtr + y * aWidth, aWidth);
                    }
                }
            }
        }

        public unsafe static void DrawBitmapFromData(int aX, int aY, int aWidth, int aHeight, Bitmap data, uint transColor)
        {
            fixed (uint* bufferPtr = &buffer[0])
            {
                fixed (int* falseImgPtr = &data.rawData[0])
                {
                    uint* imgPtr = (uint*)falseImgPtr;
                    for (int y = 0; y < aHeight; y++)
                    {
                        FastReplacer.Replace(imgPtr + y * aWidth, transColor, bufferPtr + aX + (aY + y) * width, data.rawData.Length);
                        MemoryOperations.Copy(bufferPtr + aX + (aY + y) * width, imgPtr + y * aWidth, aWidth);
                    }
                }
            }
        }

        public static void DrawString(uint x, uint y, Sys.Graphics.Fonts.Font font, char[] text, uint color = 0)
        {
            Debugger debugger = new Debugger("", "");
            if (text.Length > 0 && font != null)
            {
                int xx = (int)x;
                int yy = (int)y;

                foreach (char item in text)
                {
                    if (item == '\0')
                    {
                        break;
                    }
                    DrawCharNew(xx, yy, item, color, font);

                    if (item == '\n') { yy += font.Width + 1; xx = (int)x; }
                    else { xx += font.Height - 5; }
                }
            }
        }
        #endregion

        public static Chunk GetChunk(uint x, uint y)
        {
            for (int i = 0; i < chunks.Length; i++)
            {
                if (chunks[i].startX <= x && chunks[i].endX >= x)
                {
                    if (chunks[i].startY <= y && chunks[i].endY >= y)
                    {
                        return chunks[i];
                    }
                }
            }
            return new Chunk();
        }

        public static void ChangedInChunk(uint x, uint y)
        {
            for (int i = 0; i < chunks.Length; i++)
            {
                if (chunks[i].startX <= x && chunks[i].endX >= x)
                {
                    if (chunks[i].startY <= y && chunks[i].endY >= y)
                    {
                        chunks[i].bufferChanged = true;
                    }
                }
            }
        }
        public static Chunk[] GetChunkGrid(int row, int col, int width, int height)
        {
            Chunk[] tempChunks = new Chunk[row * col];
            uint chunksWidth = (uint)(width / col);
            uint chunksHeight = (uint)(height / row);
            int index = 0;
            for (uint i = 0; i < col; i++)
            {
                for (uint j = 0; j < row; j++)
                {
                    tempChunks[index].endX = (tempChunks[index].startX = chunksWidth * i) + chunksWidth;
                    tempChunks[index].endY = (tempChunks[index].startY = chunksHeight * j) + chunksHeight;
                    tempChunks[index].bufferChanged = true;
                    tempChunks[index].width = (int)chunksWidth;
                    tempChunks[index].height = (int)chunksHeight;
                    index++;
                }
            }
            return tempChunks;
        }
    }
}

/**
* For the brave souls who get this far: You are the chosen ones,
* the valiant knights of programming who toil away, without rest,
* fixing our most awful code. To you, true saviors, kings of men,
* I say this: never gonna give you up, never gonna let you down,
* never gonna run around and desert you. Never gonna make you cry,
* never gonna say goodbye. Never gonna tell a lie and hurt you.
*/
