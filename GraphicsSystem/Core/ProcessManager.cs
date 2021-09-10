using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Core
{
    public static class ProcessManager
    {
        public static List<Window> windows = new List<Window>();
        public static int tick = 0;
        public static int maxID = 0;

        public static void UpdateProcesses()
        {
            tick++;

            for (int i = 0; i < windows.Count; i++)
            {
                windows[i]._Update();
            }

            if (tick > 2) { tick = 0; }

        }

        public static void AddProcess(Window proc) { proc.processID = maxID++; windows.Add(proc); }

        public static void RemoveProcess(int processID)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].processID == processID)
                {
                    windows.RemoveAt(i);
                }
            }
        }

    }
}
