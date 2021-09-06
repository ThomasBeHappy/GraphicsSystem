using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using GraphicsSystem.Core;
using GraphicsSystem.Types;

namespace GraphicsSystem
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            Graphics.Initialize(mDebugger);

            mDebugger.Send("Draw Rectangles");
            Graphics.Rectangle(10, 10, 40, 40, Color.blue);
            mDebugger.Send("First Rectangle drawn into memory");
            Graphics.Rectangle(60, 60, 90, 90, Color.blue, true, Color.white, 2);
            mDebugger.Send("Done Drawing");
            Graphics.DrawCircle(60, 20, 6, Color.blue, true, Color.white, 2);
            Graphics.DrawCircle(90, 20, 6, Color.blue);
        }

        protected override void Run()
        {
            while (true)
            {
                Graphic.Taskbar.Draw();
                Graphics.Rectangle(10, 10, 200, 40, Color.blue);
                Graphics.Rectangle(60, 60, 300, 90, Color.blue, true, Color.white, 2);

                Graphics.UpdateCursor();

                //Graphics.Rectangle(10, 10, 40, 40, Color.blue);
                //Graphics.Rectangle(60, 60, 90, 90, Color.blue, true, Color.white, 2);
                //Graphics.DrawCircle(60, 20, 6, Color.blue, true, Color.white, 2);
                //Graphics.DrawCircle(90, 20, 6, Color.blue);

                Graphics.Update();
            }
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
