//using Cosmos.Core.IOGroup.ExtPack;
using Cosmos.Debug.Kernel;
using Cosmos.HAL.Drivers.PCI.Video;
using Cosmos.System.Graphics;
using GraphicsSystem.Hardware;
using GraphicsSystem.Types;
using Point = GraphicsSystem.Types.Point;
using Sys = Cosmos.System;

namespace GraphicsSystem.Core
{
    public static class Graphics
    {
        public static VMWareSVGAII driver;

        public const int width = 720, height = 480;

        public static uint[] buffer;
        private static uint[] oldBuffer;
        private static bool bufferChanged = false;
        private static Debugger _debugger;

        public static void Initialize(Debugger debugger)
        {
            _debugger = debugger;
            driver = new VMWareSVGAII();
            driver.SetMode(720, 480);
            buffer = new uint[width * height];
            oldBuffer = new uint[width * height];
            _debugger.Send(buffer.Length.ToString());
            Sys.MouseManager.ScreenWidth = width;
            Sys.MouseManager.ScreenHeight = height;

        }

        public static void Update()
        {
            if (bufferChanged)
            {
                for (int i = 0; i < width * height; i++)
                {
                    //if (buffer[i] != oldBuffer[i])
                    //{

                    int x = i % width;
                    int y = i / width;
                    //_debugger.Send(x + " " + y + " " + buffer[i]);
                    driver.SetPixel((uint)x, (uint)y, buffer[i]);

                    //}
                }
                driver.Update(0, 0, width, height);
                //oldBuffer = buffer;
                ClearBuffer(Color.gray160);
                bufferChanged = false;
            }
        }

        public static void ClearBuffer(uint color = 0)
        {
            for (int i = 0; i < width * height; i++)
            {
                if (buffer[i] != color) { buffer[i] = color; }
            }
        }

        public static void UpdateCursor()
        {
            Point position = Mouse.position;

            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    SetPixel((uint)(position.x + x), (uint)(position.y + y), Cursor.arrow[x + y * 12]);
                }
            }
        }

        #region EditBufferMethods

        public static void SetPixel(uint x, uint y, uint color)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                bufferChanged = true;

                buffer[x + y * width] = color;
            }
        }

        public static void Rectangle(uint x, uint y, uint endX, uint endY, uint color, bool border = false, uint borderColor = 0, uint borderThickness = 0)
        {
            if (x <= 0 && x > width && y <= 0 && y > height) return;

            bufferChanged = true;
            if (border)
            {
                uint _width = endX - x;
                uint _height = endY - y;

                for (int i = 0; i < _height; i++)
                {
                    for (int h = 0; h < _height; h++)
                    {
                        if (h < borderThickness || _height - h <= borderThickness)
                        {
                            buffer[(x + i) + (y + h) * width] = borderColor;
                        }else if (_width - i <= borderThickness || i < borderThickness)
                        {
                            buffer[(x + i) + (y + h) * width] = borderColor;
                        } else
                        {
                            buffer[(x + i) + (y + h) * width] = color;
                        }

                    }
                }
            }
            else
            {
                uint _width = endX - x;
                uint _height = endY - y;
                for (int i = 0; i < _width; i++)
                {
                    for (int h = 0; h < _height; h++)
                    {
                        buffer[(x + i) + (y + h) * width] = color;
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
            }else
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
        #endregion

    }

}
