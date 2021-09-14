using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.HAL;
using Cosmos.System.Helpers;

namespace GraphicsSystem.Graphic.Controls
{
    class InputField : Control
    {
        private int width, height;
        private uint x, y, color;
        char[] input;

        public InputField(int width, int height, uint x, uint y, uint color, int maxLenght = 32)
        {
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;
            input = new char[maxLenght];
        }

        
        public override void Draw()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
