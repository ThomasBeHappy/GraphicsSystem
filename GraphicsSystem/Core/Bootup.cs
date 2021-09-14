using GraphicsSystem.Data;
using GraphicsSystem.Graphic;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Core
{
    public static class Bootup
    {
        static string RTTTL = "Tetris:d=4,o=5,b=160:e6,8b,8c6,8d6,16e6,16d6,8c6,8b,a,8a,8c6,e6,8d6,8c6,b.,8c6,d6,e6,c6,a,2a,8p,d6,8f6,a6,8g6,8f6,e.6,8c6,e6,8d6,8c6,b,8b,8c6,d6,e6,c6,a,2a";


        public static void Run()
        {
            Graphics.Initialize();
            Graphics.DrawBitmapFromData(0, 0, 1920, 1080, BootBitmap.bitmap);
            Graphics.Update();
            RTTTLParser.Play(RTTTL);
            FileSystem.Initialize();
            Taskbar.Initialize();
            ProcessManager.AddProcess(new Apps.Clock(10, 10, 300, 300));
            //ProcessManager.AddProcess(new Apps.Clock(10, 10, 300, 300));
            //ProcessManager.AddProcess(new Apps.Clock(10, 10, 300, 300));
            //ProcessManager.AddProcess(new Apps.Clock(10, 10, 300, 300));
            //ProcessManager.AddProcess(new Apps.Clock(10, 10, 300, 300));
            //ProcessManager.AddProcess(new Apps.Clock(10, 10, 300, 300));
            ProcessManager.AddProcess(new Apps.FileExplorer(500, 500, 500, 400));
            ProcessManager.AddProcess(new Apps.FileExplorer(500, 500, 500, 400));
            ProcessManager.AddProcess(new Apps.FileExplorer(500, 500, 500, 400));
            ProcessManager.PrioritizeZ(1);
        }
    }
}
