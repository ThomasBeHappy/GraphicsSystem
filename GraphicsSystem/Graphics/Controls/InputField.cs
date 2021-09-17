using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.HAL;
using Cosmos.System;
using Cosmos.System.Helpers;
using GraphicsSystem.Core;
using GraphicsSystem.Types;

namespace GraphicsSystem.Graphic.Controls
{
    class InputField : Control
    {
        private int width, height;
        private uint x, y, color;
        private uint textColor;
        List<char> input;
        public Window window;
        private uint drawColor;
        FontMono7x9 font = new FontMono7x9();

        public bool focused = false;

        public InputField(Window app, int width, int height, uint x, uint y, uint color, uint textColor, uint drawColor, int maxLenght = 32)
        {
            this.window = app;
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;
            this.textColor = textColor;
            this.drawColor = drawColor;
            input = new List<char>();
        }

        List<char> drawText = new List<char>();
        public override void Draw()
        {
            Graphics.Rectangle(window.x + x, window.y + y, (uint)(window.x + x + width), (uint)(window.y + y + height), drawColor);

            drawText = input;
            while (font.characterWidth * drawText.Count > width)
            {
                drawText.RemoveAt(0);
            }

            Graphics.DrawString(window.x + x, (uint)(window.y + y + (font.characterHeight / 2)), font, drawText.ToArray(), textColor);
        }

        public override void Update()
        {
            if (focused)
            {
                if (System.Console.KeyAvailable)
                {
                    KeyEvent keyEvent;
                    if (KeyboardManager.TryReadKey(out keyEvent))
                    {
                        switch (keyEvent.Key)
                        {
                            case ConsoleKeyEx.Enter:
                                this.input.Add('\n');
                                break;
                            case ConsoleKeyEx.Backspace:
                                if (this.input.Count != 0)
                                {
                                    input.RemoveAt(input.Count - 1);
                                }
                                break;
                            default:
                                input.Add(keyEvent.KeyChar);
                                break;
                        }
                    }
                }
            }
        }
    }
}
