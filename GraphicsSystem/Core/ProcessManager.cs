using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Core
{
    public static class ProcessManager
    {
        public static List<SystemProcess> processes = new List<SystemProcess>();
        public static int tick = 0;

        public static void UpdateProcesses()
        {
            tick++;


            for (int i = 0; i < processes.Count; i++)
            {
                processes[i].id = i;

                if (processes[i].exitRequest)
                {
                    processes.RemoveAt(i);
                    i--;
                    continue;
                }

                switch (processes[i].priority)
                {
                    case Priority.low:
                        if (tick > 2)
                        {
                            processes[i].Update();
                        }
                        break;
                    case Priority.normal:
                        if (tick <= 1)
                        {
                            processes[i].Update();
                        }
                        break;
                    case Priority.high:
                        processes[i].Update();
                        break;
                    default:
                        break;
                }

                if (processes[i].topMost) { processes[i].Draw(); }

            }

            if (tick > 2) { tick = 0; }

        }

        public static void AddProcess(SystemProcess proc) { processes.Add(proc); }

        public static void RemoveProcess(int processID)
        {
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].id == processID)
                {
                    processes.RemoveAt(i);
                }
            }
        }

    }
}
