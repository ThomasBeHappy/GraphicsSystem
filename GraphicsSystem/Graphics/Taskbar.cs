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
                ProcessManager.windows[i].dockX = (uint)(65 + (50 * i) + devider);
                ProcessManager.windows[i].dockY = Graphics.height - 55;
                ProcessManager.windows[i].dockHeight = 50;
                ProcessManager.windows[i].dockWidth = 50;
                //Graphics.Rectangle(ProcessManager.windows[i].dockX, ProcessManager.windows[i].dockY, ProcessManager.windows[i].dockX + 60, ProcessManager.windows[i].dockY + 50, Color.yellow);
                Graphics.DrawBitmapFromData((int)ProcessManager.windows[i].dockX, (int)ProcessManager.windows[i].dockY, 50, 50, ProcessManager.windows[i].icon, Color.FromARGB(0, 0, 0, 255));
            }

            //Graphics.DrawAngle(500, 500, 30, 40, Color.red);
            //Graphics.DrawAngle(500, 500, 60, 40, Color.red);
            //Graphics.DrawAngle(500, 500, 90, 40, Color.red);
            //Graphics.DrawAngle(500, 500, 120, 40, Color.red);
            //Graphics.DrawAngle(500, 500, 150, 40, Color.red);
            //Graphics.DrawAngle(500, 500, 180, 40, Color.red);
            //Graphics.DrawAngle(500, 500, 210, 40, Color.red);
            //Graphics.DrawAngle(500, 500, 240, 40, Color.red);
            //Graphics.DrawAngle(500, 500, 270, 40, Color.red);


        }

        public static void Initialize()
        {
            logo = LogoBitmap.GetImage();
        }
    }
}
