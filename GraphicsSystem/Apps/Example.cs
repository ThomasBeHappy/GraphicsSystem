using GraphicsSystem.Core;
using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Apps
{
    public class Example : Window
    {
        static char[] name = new char[] { 'E', 'x', 'a', 'm', 'p', 'l', 'e', '\0' };

        public Example(uint x, uint y, uint width, uint height) : base(name, x, y, width, height)
        {
        }

        public override void Update()
        {
            Graphics.Rectangle(x - appOffset, y, (x + width / 2) - appOffset, y + height / 2, Color.red);
        }
    }
}
