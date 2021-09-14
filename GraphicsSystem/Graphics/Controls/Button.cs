#define COSMOSDEBUG
using Cosmos.Debug.Kernel;
using Cosmos.System;
using GraphicsSystem.Core;
using GraphicsSystem.Hardware;
using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Graphic.Controls
{
    public class Button : Control
    {
        private int width, height;
        private uint x, y;

        private EventHandler OnClickEventHandler;

        private char[] text;

        public Window window;


        private uint color;
        private uint hoverColor;
        private uint textColor;

        private uint drawColor;

        private Font font;

        public object value;

        public Button(Window app, int width, int height, uint x, uint y, uint textColor, uint color, uint hoverColor, Font font, char[] text, object value)
        {
            this.window = app;
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;
            this.textColor = textColor;
            this.color = color;
            this.font = font;
            this.text = text;
            this.hoverColor = hoverColor;
            this.value = value;

            drawColor = this.color;
        }

        public event EventHandler OnClick
        {
            add
            {
                OnClickEventHandler = value;
            }
            remove
            {
                OnClickEventHandler -= value;
            }
        }

        public override void Draw()
        {
            Graphics.Rectangle(window.x + x, window.y + y, (uint)(window.x + x + width), (uint)(window.y + y + height), drawColor);
            Graphics.DrawString(window.x + x, (uint)((uint)(window.y + y + (height/2)) - (font.characterHeight / 2)), font, text, textColor);
        }


        private bool pressed;
        public override void Update()
        {
            if (Mouse.position.x > window.x + x && Mouse.position.x < window.x + x + width && Mouse.position.y > window.y + y && Mouse.position.y < window.y + y + height)
            {
                drawColor = hoverColor;
                if (MouseManager.MouseState == MouseState.Left && pressed == false)
                {
                    pressed = true;
                    if (OnClickEventHandler != null)
                    {
                        ButtonEventArgs args = new ButtonEventArgs();
                        args.value = value;
                        OnClickEventHandler.Invoke(this, args);
                    }
                }else if (MouseManager.MouseState == MouseState.None && pressed == true)
                {
                    pressed = false;
                }
            }else
            {
                drawColor = color;
            }
        }
    }

    public class ButtonEventArgs : EventArgs
    {
        public object value;
    }
}
