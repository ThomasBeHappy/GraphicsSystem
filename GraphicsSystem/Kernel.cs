using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using GraphicsSystem.Core;
using GraphicsSystem.Types;
using GraphicsSystem.Math;
using GraphicsSystem.Graphic;
using GraphicsSystem.Data;

namespace GraphicsSystem
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            Graphics.Initialize(mDebugger);
            Taskbar.Initialize();

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

                    // Base System Draws

                    Graphics.DrawBitmapFromData(0, 0, 1920, 1080, BackgroundBitmap.bitmap);

                    //ProcessManager.UpdateProcesses();

                    // Draw Task Bar
                    Taskbar.Draw();

                    // Update and Draw Mouse
                    Graphics.UpdateCursor(); 

                    // Draw FPS counter
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

        private void Panic(Exception e)
        {
            mDebugger.Send(e.Message);
            Graphics.ClearBuffer(Color.lightBlue);
            //Graphics.DrawString(10, 10, new FontMono9x11(), "FATAL ERROR: " + e.Message);
        }
    }

    public static class Console
    {
        public static void BeforeRun()
        {

        }

        public static void Run()
        {

        }
    }
}
