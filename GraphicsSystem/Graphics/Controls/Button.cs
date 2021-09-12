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

        public Button(Window app, int width, int height, uint x, uint y, uint textColor, uint color, uint hoverColor, Font font, char[] text)
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
            Graphics.Rectangle((uint)(window.appX + x), (uint)(window.appY + y), (uint)(window.appX + x + width), (uint)(window.appY + y + height), drawColor);
            Graphics.DrawString((uint)(window.appX + x), (uint)(window.appY + y + (font.characterHeight / 2)), font, text, textColor);
        }

        public override void Update()
        {
            if (Mouse.position.x > window.appX + x && Mouse.position.x < window.appX + x + width && Mouse.position.y > window.appY + y && Mouse.position.y < window.appY + y + height)
            {
                drawColor = hoverColor;
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (OnClickEventHandler != null)
                        OnClickEventHandler.Invoke(this, new EventArgs());
                }
            }else
            {
                drawColor = color;
            }
        }
    }
}
