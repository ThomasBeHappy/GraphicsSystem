#define COSMOSDEBUG
using Cosmos.Debug.Kernel;
using Cosmos.System;
using GraphicsSystem.Core;
using GraphicsSystem.Hardware;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Types
{
    public abstract class Window
    {
        protected uint x, y,  width, height;

        public uint dockX, dockY;
        public uint dockWidth = 40, dockHeight = 30;

        public char[] name;

        protected uint appX, appY, appWidth, appHeight, appOffset = 0;

        private bool pressed;


        public bool visible = false;

        public int _i = 0;

        private uint mouseMoveOffsetX = 0;
        private uint mouseMoveOffsetY = 0;
        private uint tempOffset = 0;

        FontMono9x11 font = new FontMono9x11();

        public Window(char[] name, uint x, uint y, uint width, uint height)
        {
            this.appX = x;
            this.appY = y;
            this.width = width;
            this.height = height;

            this.y = y + 22;
            this.x = x + 2;
            this.appWidth = width - 4;
            this.appHeight = height - 23;
            this.name = name;
        }

        public void _Update()
        {
            if (_i != 0)
            {
                _i--;
            }

            if (MouseManager.X > dockX && MouseManager.X < dockX + dockWidth && MouseManager.Y > dockY && MouseManager.Y < dockY + dockHeight)
            {
                Graphics.DrawString((uint)(dockX - ((name.Length * 8) / 2) + dockWidth / 2), dockY - 20, font, name, Color.white);
            }

            if (MouseManager.MouseState == MouseState.Left && _i == 0)
            {
                if (MouseManager.X > dockX && MouseManager.X < dockX + dockWidth && MouseManager.Y > dockY && MouseManager.Y < dockY + dockHeight)
                {
                    visible = !visible;
                    _i = 60;
                }
            }

            if (Mouse.pressed)
            {
                if (MouseManager.X > appX - appOffset && MouseManager.X < appX + width - 22 - appOffset && MouseManager.Y > appY && MouseManager.Y < appY + 22)
                {
                    if (this.pressed == false)
                    {
                        mouseMoveOffsetX = MouseManager.X - appX;
                        mouseMoveOffsetY = MouseManager.Y - appY;
                    }
                    this.pressed = true;

                }
            }
            else
            {
                mouseMoveOffsetX = 0;
                mouseMoveOffsetY = 0;
                this.pressed = false;
            }

            if (!visible)
                goto end;

            if (this.pressed)
            {
                if (MouseManager.X + (appWidth - mouseMoveOffsetX) > Graphics.width)
                {
                    tempOffset = MouseManager.X + (appWidth - mouseMoveOffsetX) - Graphics.width;
                }else if(MouseManager.X - mouseMoveOffsetX < 0)
                {
                    tempOffset = MouseManager.X - mouseMoveOffsetX;
                }
                this.appX = MouseManager.X - mouseMoveOffsetX - tempOffset;
                Debugger debugger = new Debugger("", "");
                debugger.SendInternal(appX + " " + mouseMoveOffsetX);

                this.appY = MouseManager.Y - mouseMoveOffsetY;

                this.x = MouseManager.X + 2 - mouseMoveOffsetX - tempOffset;
                this.y = MouseManager.Y + 22 - mouseMoveOffsetY;
            }
            //debugger.SendInternal(appX + "");
            //if (appX + appWidth > Graphics.width)
            //{
            //    appOffset = (appX + appWidth) - Graphics.width + 1;
            //}else if (appX < 0)
            //{
            //    appOffset = appX;
            //    debugger.SendInternal(appX + " " + (appX - appOffset));
            //}
            //else
            //{
            //    appOffset = 0;
            //}
            Graphics.Rectangle(appX - appOffset, appY, appX + width - appOffset, appY + height, Color.gray32);
            Graphics.Rectangle(appX - appOffset, appY, appX + width - appOffset, appY + 22, Color.black);
            Graphics.Rectangle(appX + width - 22 - appOffset, appY, appX + width - appOffset, appY + 22, Color.red);
            Graphics.Rectangle(appX - appOffset, appY, appX - appOffset + 22, appY + 22, Color.black);

            Update();
            end:;
        }

        public virtual void Update()
        {

        }
    }
}
