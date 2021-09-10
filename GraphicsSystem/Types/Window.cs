#define COSMOSDEBUG
using Cosmos.Debug.Kernel;
using Cosmos.System;
using Cosmos.System.Graphics;
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

        protected int appX, appY, appWidth, appHeight, appOffset = 0;

        private bool pressed;


        public bool visible = false;

        public int _i = 0;

        private uint mouseMoveOffsetX = 0;
        private uint mouseMoveOffsetY = 0;
        private int tempOffset = 0, tempOffsetY = 0;

        FontMono9x11 font = new FontMono9x11();

        uint quitColor = Color.white;

        public Bitmap icon;

        public int processID = 0;

        public Window(char[] name, uint x, uint y, uint width, uint height, Bitmap icon)
        {
            this.appX = (int)x;
            this.appY = (int)y;
            this.appWidth = (int)width;
            this.appHeight = (int)height;

            this.y = y + 22;
            this.x = x + 2;
            this.width = width - 4;
            this.height = height - 24;
            this.name = name;

            this.icon = icon;
        }

        public void _Update()
        {
            if (_i != 0)
            {
                _i--;
            }
            // Draw dock string when hovering over app icon
            if (MouseManager.X > dockX && MouseManager.X < dockX + dockWidth && MouseManager.Y > dockY && MouseManager.Y < dockY + dockHeight)
            {
                Graphics.DrawString((uint)(dockX - ((name.Length * 8) / 2) + dockWidth / 2), dockY - 20, font, name, Color.white);
            }


            // Turn app visible or invisible when clicking the app icon
            if (MouseManager.MouseState == MouseState.Left && _i == 0)
            {
                if (MouseManager.X > dockX && MouseManager.X < dockX + dockWidth && MouseManager.Y > dockY && MouseManager.Y < dockY + dockHeight)
                {
                    visible = !visible;
                    _i = 60;
                }
            }

            // Handle the quit button on the app
            if (MouseManager.X > appX + width - 22 && MouseManager.X < appX + width && MouseManager.Y > appY && MouseManager.Y < appY + 22)
            {
                quitColor = Color.gray160;
                if (MouseManager.MouseState == MouseState.Left && this.pressed == false)
                {
                    Quit();
                    ProcessManager.RemoveProcess(processID);
                }
            }else
            {
                quitColor = Color.white;
            }

            // Move the app
            if (Mouse.pressed)
            {
                if (MouseManager.X > appX - appOffset && MouseManager.X < appX + width - 22 - appOffset && MouseManager.Y > appY && MouseManager.Y < appY + 22)
                {
                    if (this.pressed == false)
                    {
                        mouseMoveOffsetX = (uint)(MouseManager.X - appX);
                        mouseMoveOffsetY = (uint)(MouseManager.Y - appY);
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
                    tempOffset = (int)(MouseManager.X + (appWidth - mouseMoveOffsetX) - Graphics.width) + 10;
                }else if((int)MouseManager.X - (int)mouseMoveOffsetX < 0)
                {
                    tempOffset = (int)MouseManager.X - (int)mouseMoveOffsetX;
                }else
                {
                    tempOffset = 0;
                }

                if ((int)MouseManager.Y - (int)mouseMoveOffsetY < 0)
                {
                    tempOffsetY = (int)MouseManager.Y - (int)mouseMoveOffsetY;
                }else if (MouseManager.Y + (appHeight - mouseMoveOffsetY) > Graphics.height)
                {
                    tempOffsetY = (int)(MouseManager.Y + (appHeight - mouseMoveOffsetY) - Graphics.height);
                }else
                {
                    tempOffsetY = 0;
                }

                this.appX = (int)(MouseManager.X - mouseMoveOffsetX - tempOffset);
                //Debugger debugger = new Debugger("", "");
                //debugger.SendInternal(appX + " " + mouseMoveOffsetX);

                this.appY = (int)(MouseManager.Y - mouseMoveOffsetY - tempOffsetY);

                this.x = (uint)(MouseManager.X + 2 - mouseMoveOffsetX - tempOffset);
                this.y = (uint)(MouseManager.Y + 22 - mouseMoveOffsetY - tempOffsetY);
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

            Graphics.Rectangle((uint)appX, (uint)appY, (uint)(appX + appWidth), (uint)(appY + appHeight), Color.black);
            Graphics.Rectangle(x, y, (x + width), (y + height), Color.gray32);
            Graphics.Rectangle((uint)(appX), (uint)appY, (uint)(appX + width), (uint)(appY + 22), Color.black);
            Graphics.Rectangle((uint)(appX + width - 22), (uint)appY + 5, (uint)(appX + width - 5), (uint)(appY + 22), quitColor);
            Graphics.DrawString((uint)appX, (uint)(appY + (font.characterHeight / 2)), font, name, Color.white);

            Update();
            end:;
        }

        public virtual void Update()
        {

        }

        public virtual void Quit()
        {

        }
    }
}
