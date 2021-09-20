using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using GraphicsSystem.Core;
using GraphicsSystem.Types;
using GraphicsSystem.Math;
using GraphicsSystem.Graphic;
using GraphicsSystem.Data;
using GraphicsSystem.Apps;
using Cosmos.System;
using GraphicsSystem.Hardware;
using Cosmos.System.Graphics;

namespace GraphicsSystem
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            Bootup.Run();
        }

        public char[] fps = new char[] { 'F', 'P', 'S', ':' , ' ', '\0' };
        public char[] final = new char[32];
        public char[] number = new char[32];
        Font font = new FontMono9x11();
        protected override void Run()
        {
            try
            {
                while (true)
                {
                    // Important that this is done before anything else.
                    switch (MouseManager.MouseState)
                    {
                        case MouseState.Left:
                            Mouse.pressed = true;
                            break;
                        case MouseState.None:
                            Mouse.pressed = false;
                            break;
                    }

                    // Base System Draws
                    // First thing to draw is the background image, as everything is allowed to overlap this.
                    Graphics.DrawBitmapFromData(0, 0, 1920, 1080, BootBitmap.bitmap);

                    // Then draw all the windows that are loaded and update these windows.
                    ProcessManager.UpdateProcesses();

                    // Now draw the Taskbar which should always be over the windows.
                    Taskbar.Draw();

                    if (GifData.gif.currentIndex < GifData.gif.images.Count - 1)
                    {
                        Graphics.DrawBitmapFromData(10, 10, (int)GifData.gif.images[GifData.gif.currentIndex].width, (int)GifData.gif.images[GifData.gif.currentIndex].height, GifData.gif.images[GifData.gif.currentIndex].imageData);
                        GifData.gif.currentIndex++;
                    }
                    else
                    {
                        GifData.gif.currentIndex = 0;
                    }

                    // Update and Draw Mouse
                    // Next is the Mouse which should overlap almost everything.
                    Graphics.UpdateCursor();

                    // Draw FPS counter
                    // Draw a FPS counter to keep track of FPS, draw this above the mouse to avoid confusion with applications.
                    InternalString.IntToString(Graphics.fps, ref number);
                    InternalString.combineString(ref fps, ref number, ref final);
                    Graphics.DrawString(0, 0, font, final, Color.white);

                    Graphics.Update();
                }
            }
            catch (Exception e)
            {

                Panic(e);
            }
        }

        public void Panic(Exception e)
        {
            mDebugger.Send(e.Message);
            Graphics.ClearBuffer(Color.lightBlue);
            Graphics.DrawString(10, 10, new FontMono9x11(), ("FATAL ERROR: " + e).ToCharArray());
            Graphics.Update();
        }
    }
}
