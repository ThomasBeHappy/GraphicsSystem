using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;
using GraphicsSystem.Core;
using Cosmos.System.Graphics;
using GraphicsSystem.Data;

namespace GraphicsSystem.Graphic
{
    public static class Taskbar
    {
        public static Bitmap logo;
        public static void Draw()
        {
            //Graphics.Rectangle(0, Graphics.height - 60, Graphics.width, Graphics.height, Color.darkBlue);
            //Graphics.Rectangle(101, Graphics.height - 59, 110, Graphics.height - 1, Color.black);
            //Graphics.DrawBitmapFromData(0, Graphics.height - 50, 50, 50, logo);
            Graphics.DrawBitmapFromData(0, 0, 1920, 1080, BackgroundBitmap.bitmap);
        }

        public static void Initialize()
        {
            logo = LogoBitmap.GetImage();
        }
    }
}
