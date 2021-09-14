using GraphicsSystem.Data;
using GraphicsSystem.Graphic;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Core
{
    public static class Bootup
    {
        public static void Run()
        {
            Graphics.Initialize();
            Graphics.DrawBitmapFromData(0, 0, 1920, 1080, BootBitmap.bitmap);
            Graphics.Update();
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
