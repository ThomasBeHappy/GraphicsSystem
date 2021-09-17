using Cosmos.Debug.Kernel;
using Cosmos.System;
using Cosmos.System.Graphics;
using GraphicsSystem.Core;
using GraphicsSystem.Data;
using GraphicsSystem.Graphic.Controls;
using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Apps
{
    class Notepad : Window
    {
        static char[] name = new char[] { 'N', 'o', 't', 'e', 'p', 'a', 'd', '-', '-', '\0' };
        List<Control> controls = new List<Control>();
        static Bitmap icon = ProgramBitmaps.bitmap;
        List<char> input = new List<char>();
        List<char> drawText = new List<char>();
        FontMono7x9 font = new FontMono7x9();
        public Notepad(uint x, uint y, uint width, uint height) : base(name, x, y, width, height, icon)
        {
        }

        public override void Update()
        {
            if (this.Z_Index == 0)
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

            Graphics.DrawString(this.x + 10, (uint)(this.y + 10), font, input.ToArray(), Color.white);
        }
    }
}
