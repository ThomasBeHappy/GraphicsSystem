using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Graphic.Controls
{
    class InputField : Control
    {
        private int width, height;
        private uint x, y, color;

        public InputField(int width, int height, uint x, uint y, uint color)
        {
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;
        }

        
        public override void Draw()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
