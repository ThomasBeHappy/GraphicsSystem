using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;
using GraphicsSystem.Core;

namespace GraphicsSystem.Graphic
{
    public static class Taskbar
    {
        public static void Draw()
        {
            Graphics.Rectangle(0, Graphics.height - 40, Graphics.width, Graphics.height, Color.darkBlue);
            Graphics.Rectangle(30, Graphics.height - 39, 35, Graphics.height - 1, Color.black);

        }
    }
}
