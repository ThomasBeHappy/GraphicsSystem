using Cosmos.Core;
using Cosmos.Debug.Kernel;
using Cosmos.HAL;
using Cosmos.HAL.Drivers.PCI.Video_plug;
using Cosmos.System.Graphics;
using GraphicsSystem.Hardware;
using GraphicsSystem.Types;
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
        private static bool bufferChanged = false;
        private static Debugger _debugger;

        public static Chunk[] chunks = new Chunk[6];

        private static int frames = 0;
        public static int fps { get; private set; } = 0;
        public static float delta { get; private set; } = 0;
        private static int tick = 0;

        public static void Initialize(Debugger debugger)
        {
            _debugger = debugger;
            driver = new VMWareSVGAII();
            driver.SetMode(width, height);
            buffer = new uint[width * height];
            oldBuffer = new uint[width * height];
            //_debugger.Send(buffer.Length.ToString());
            Sys.MouseManager.ScreenWidth = width;
            Sys.MouseManager.ScreenHeight = height;
            ClearBuffer(Color.gray160);
            bufferChanged = true;
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

            // if (bufferChanged)
            // {
            //     bool actualChange = false;
            //     for (int i = 0; i < width * height; i++)
            //     {
            //         if (buffer[i] != oldBuffer[i])
            //         {

            //             int x = i % width;
            //             int y = i / width;
            //             driver.SetPixel((uint)x, (uint)y, buffer[i]);
            //             actualChange = true;
            //         }
            //         oldBuffer[i] = buffer[i];
            //     }
            //     if (actualChange)
            //     {
            //     }

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

        public static void DrawLine(int aX, int aY, int endX, int endY, uint color)
        {
            int x0 = aX;
            int x1 = endX;
            int y0 = aY;
            int y1 = endY;
            bool steep = System.Math.Abs(y1 - y0) > System.Math.Abs(x1 - x0);
            if (steep)
            {
                int x3 = y0;
                y0 = x0;
                x0 = x3;
                int x4 = y1;
                y1 = x1;
                x1 = x4;
            }
            if (x0 > x1)
            {
                int x5 = x0;
                x0 = x1;
                x1 = x5;
                int x6 = y0;
                y0 = y1;
                y1 = x6;
            }
            int deltax = x1 - x0;
            int deltay = System.Math.Abs(y1 - y0);
            int error = deltax / 2;
            int ystep;
            int y = y0;
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

        static int[] sine = new int[16] { 0, 27, 54, 79, 104, 128, 150, 171, 190, 201, 221, 233, 243, 250, 254, 255 };
        static int xEnd, yEnd, quadrant, x_flip, y_flip;

        public static void DrawAngle(int X, int Y, int angle, int radius, uint color)
        {
            //quadrant = angle / 15;
            //switch (quadrant)
            //{
            //    case 0: x_flip = 1; y_flip = -1; break;
            //    case 1: angle = System.Math.Abs(angle - 30); x_flip = y_flip = 1; break;
            //    case 2: angle -= 30; x_flip = -1; y_flip = 1; break;
            //    case 3: angle = System.Math.Abs(angle - 60); x_flip = y_flip = -1; break;
            //    default: x_flip = y_flip = 1; break;
            //}
            //xEnd = X;
            //yEnd = Y;
            //if (angle > sine.Length) return;
            //xEnd += (x_flip * ((sine[angle] * radius) >> 8));
            //yEnd += (y_flip * ((sine[15 - angle] * radius) >> 8));


            var x = radius * System.Math.Sin(System.Math.PI * 2 * angle / 360);
            var y = radius * System.Math.Cos(System.Math.PI * 2 * angle / 360);

            DrawLine(X, Y, X + (int)(System.Math.Round(x*100)/100), Y + (int)(System.Math.Round(y * 100) / 100), color);
        }

        public static void SetPixel(uint x, uint y, uint color)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                if (x + y * width > width * height)
                {
                    throw new System.Exception("Tried setting a pixel outside of the screen width and height");
                }else
                {
                    if (buffer[x + y * width] != color)
                    {
                        bufferChanged = true;
                        buffer[x + y * width] = color;
                    }
                }
            }
        }

        public unsafe static void Rectangle(uint x, uint y, uint endX, uint endY, uint color, bool border = false, uint borderColor = 0, uint borderThickness = 0)
        {
            if (x <= 0 && x > width && y <= 0 && y > height) return;

            bufferChanged = true;
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


            bufferChanged = true;
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
            bufferChanged = true;

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


        public static void DrawChar(int x, int y, char c, uint color, Font font)
        {
            int width = font.characterWidth;
            int height = font.characterHeight;

            if(c == '!') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.exclamation]); }
            else if(c == '"') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.quotation]); }
            else if(c == '#') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.numberSign]); }
            else if(c == '$') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.dollarSign]); }
            else if(c == '%') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.percent]); }
            else if(c == '&') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.ampersand]); }
            else if(c == '\'') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.apostrophe]); }
            else if(c == '(') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.bracketLeft]); }
            else if(c == ')') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.bracketRight]); }
            else if(c == '*') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.multiply]); }
            else if(c == '+') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.add]); }
            else if(c == ',') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.comma]); }
            else if(c == '-') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.minus]); }
            else if(c == '.') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.period]); }
            else if(c == '/') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.slash]); }
            else if(c == '1') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n1]); }
            else if(c == '2') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n2]); }
            else if(c == '3') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n3]); }
            else if(c == '4') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n4]); }
            else if(c == '5') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n5]); }
            else if(c == '6') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n6]); }
            else if(c == '7') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n7]); }
            else if(c == '8') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n8]); }
            else if(c == '9') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n9]); }
            else if(c == '0') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n0]); }
            else if(c == ':') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.colon]); }
            else if(c == ';') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.semiColon]); }
            else if(c == '<') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.arrowLeft]); }
            else if(c == '=') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.equals]); }
            else if(c == '>') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.arrowRight]); }
            else if(c == '?') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.question]); }
            else if(c == '@') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.at]); }
            else if(c == 'A') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capA]); }
            else if(c == 'B') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capB]); }
            else if(c == 'C') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capC]); }
            else if(c == 'D') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capD]); }
            else if(c == 'E') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capE]); }
            else if(c == 'F') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capF]); }
            else if(c == 'G') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capG]); }
            else if(c == 'H') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capH]); }
            else if(c == 'I') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capI]); }
            else if(c == 'J') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capJ]); }
            else if(c == 'K') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capK]); }
            else if(c == 'L') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capL]); }
            else if(c == 'M') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capM]); }
            else if(c == 'N') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capN]); }
            else if(c == 'O') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capO]); }
            else if(c == 'P') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capP]); }
            else if(c == 'Q') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capQ]); }
            else if(c == 'R') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capR]); }
            else if(c == 'S') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capS]); }
            else if(c == 'T') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capT]); }
            else if(c == 'U') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capU]); }
            else if(c == 'V') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capV]); }
            else if(c == 'W') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capW]); }
            else if(c == 'X') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capX]); }
            else if(c == 'Y') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capY]); }
            else if(c == 'Z') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.capZ]); }
            else if(c == '[') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.sqBracketL]); }
            else if(c == '\\') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.backSlash]); }
            else if(c == ']') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.sqBracketR]); }
            else if(c == '^') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.upArrow]); }
            else if(c == '_') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.underscore]); }
            else if(c == '`') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.tilde]); }
            else if(c == 'a') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.a]); }
            else if(c == 'b') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.b]); }
            else if(c == 'c') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.c]); }
            else if(c == 'd') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.d]); }
            else if(c == 'e') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.e]); }
            else if(c == 'f') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.f]); }
            else if(c == 'g') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.g]); }
            else if(c == 'h') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.h]); }
            else if(c == 'i') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.i]); }
            else if(c == 'j') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.j]); }
            else if(c == 'k') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.k]); }
            else if(c == 'l') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.l]); }
            else if(c == 'm') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.m]); }
            else if(c == 'n') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.n]); }
            else if(c == 'o') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.o]); }
            else if(c == 'p') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.p]); }
            else if(c == 'q') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.q]); }
            else if(c == 'r') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.r]); }
            else if(c == 's') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.s]); }
            else if(c == 't') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.t]); }
            else if(c == 'u') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.u]); }
            else if(c == 'v') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.v]); }
            else if(c == 'w') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.w]); }
            else if(c == 'x') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.x]); }
            else if(c == 'y') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.y]); }
            else if(c == 'z') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.z]); }
            else if(c == '{') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.crBracketL]); }
            else if(c == '|') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.div]); }
            else if(c == '}') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.crBracketR]); }
            else if(c == '~') { DrawBitmap(x, y, width, height, color, font.characters[FontCharIndex.squiggle]); }
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
            fixed(uint* bufferPtr = &buffer[0]){
                fixed(int* falseImgPtr = &data.rawData[0]){
                    uint* imgPtr = (uint*)falseImgPtr;
                    for (int y = 0; y < aHeight; y++)
                    {
                        MemoryOperations.Copy(bufferPtr + aX + (aY + y) * width, imgPtr + y * aWidth, aWidth);
                    }
                }
            }
        }

        public static void DrawBitmapFromData(int aX, int aY, int aWidth, int aHeight, Bitmap data, uint transColor)
        {
            for (int xx = 0; xx < aWidth; xx++)
            {
                for (int yy = 0; yy < aHeight; yy++)
                {
                    if (data.rawData[xx + yy * aWidth] != transColor)
                    {
                        buffer[(aX + xx) + (aY + yy) * width] = (uint)data.rawData[xx + yy * aWidth];
                    }
                }
            }
        }

        public static void DrawString(uint x, uint y, Font font, char[] text, uint color = 0)
        {
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
                    DrawChar(xx, yy, item, color, font);

                    if (item == '\n') { yy += font.characterHeight + 1; xx = (int)x; }
                    else { xx += font.characterWidth + FONT_SPACING; }
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
