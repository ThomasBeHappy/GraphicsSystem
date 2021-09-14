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

            BubbleSort(windows);

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

        public static void PrioritizeZ(int processID)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].processID == processID)
                {
                    windows[i].Z_Index = 0;
                }else
                {
                    windows[i].Z_Index = windows[i].Z_Index + 1;
                }
            }
        }

        static void BubbleSort(List<Window> arr)
        {
            int n = arr.Count;
            for (int i = 0; i < n - 1; i++)
                for (int j = 0; j < n - i - 1; j++)
                    if (arr[j].Z_Index < arr[j + 1].Z_Index)
                    {
                        // swap temp and arr[i]
                        Window temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
        }

    }
}
