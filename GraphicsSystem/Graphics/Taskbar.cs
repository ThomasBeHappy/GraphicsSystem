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

        static uint devider = 30;
        public static void Draw()
        {
            // Taskbar
            Graphics.Rectangle(0, Graphics.height - 60, Graphics.width, Graphics.height, Color.darkBlue);
            Graphics.DrawBitmapFromData(0, Graphics.height - 55, 50, 50, logo, 0);
            Clock.Draw();
            Graphics.Rectangle(55, Graphics.height - 60, 60, Graphics.height, Color.black);

            for (int i = 0; i < ProcessManager.windows.Count; i++)
            {
                ProcessManager.windows[i].dockX = (uint)(65 + (50 * (i + devider)));
                ProcessManager.windows[i].dockY = Graphics.height - 55;
                ProcessManager.windows[i].dockHeight = 50;
                ProcessManager.windows[i].dockWidth = 50;
                Graphics.DrawBitmapFromData((int)ProcessManager.windows[i].dockX, (int)ProcessManager.windows[i].dockY, 50, 50, ProcessManager.windows[i].icon, Color.FromARGB(0, 0, 0, 255));
            }
        }

        public static void Initialize()
        {
            logo = LogoBitmap.GetImage();
        }
    }
}
