using GraphicsSystem.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Types
{
    public class Window
    {
        uint x, y, width, height;

        public Window(uint x, uint y, uint width, uint height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void Draw()
        {
            Graphics.Rectangle(x, y, width, height, Color.gray32, true, Color.black, 5);
            Graphics.Rectangle(x, y, width, 10, Color.black);
            Graphics.Rectangle(width-10, y, width, 10, Color.red);
        }
    }
}
