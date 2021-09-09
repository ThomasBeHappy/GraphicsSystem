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

        static uint devider = 10;
        public static void Draw()
        {
            // Taskbar
            Graphics.Rectangle(0, Graphics.height - 60, Graphics.width, Graphics.height, Color.darkBlue);
            Graphics.DrawBitmapFromData(0, Graphics.height - 55, 50, 50, logo, 0);
            Clock.Draw();

            for (int i = 0; i < ProcessManager.windows.Count; i++)
            {
                ProcessManager.windows[i].dockX = (uint)(60 + (devider * i));
                ProcessManager.windows[i].dockY = (uint)(Graphics.height - 55);
                ProcessManager.windows[i].dockHeight = 50;
                ProcessManager.windows[i].dockWidth = 60;
                Graphics.Rectangle(ProcessManager.windows[i].dockX, ProcessManager.windows[i].dockY, ProcessManager.windows[i].dockX + 60, ProcessManager.windows[i].dockY + 50, Color.yellow);
            }

        }

        public static void Initialize()
        {
            logo = LogoBitmap.GetImage();
        }
    }
}
