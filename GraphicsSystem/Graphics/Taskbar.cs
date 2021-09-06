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
            //Graphics.Rectangle(20, Graphics.height - 41, 22, 1, Color.black);

        }
    }
}
