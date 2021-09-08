using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;
using GraphicsSystem.Core;
using Cosmos.System.Graphics;
using GraphicsSystem.Data;
using Cosmos.HAL;

namespace GraphicsSystem.Graphic
{
    public static class Taskbar
    {
        public static Bitmap logo;

        static int maxAngle24 = 8640;
        static int maxAngle60 = 21600;
        public static void Draw()
        {
            // Taskbar
            Graphics.Rectangle(0, Graphics.height - 60, Graphics.width, Graphics.height, Color.darkBlue);
            Graphics.DrawBitmapFromData(0, Graphics.height - 55, 50, 50, logo, 0);
            Clock.Draw();

            // Draw Temp Clock
            //Graphics.Rectangle(400, 400, 600, 600, Color.black);
            //Graphics.DrawLine(100, 100, 200, 130, Color.red);

            //Graphics.DrawAngle(500, 500, 30, 40, Color.white);
            //Graphics.DrawAngle(500, 500, 60, 60, Color.white);
            //Graphics.DrawAngle(500, 500, 90, 80, Color.red);
        }

        public static void Initialize()
        {
            logo = LogoBitmap.GetImage();
        }
    }
}
