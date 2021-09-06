using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Types
{
    public enum ProcessType
    {
        normal,
        system,
        window,
    }

    public abstract class Process
    {
        public string name;
        public int id;
        public Priority priority;
        public ProcessType type;
        public bool exitRequest = false;

        public Process(string name)
        {
            this.name = name;
            this.type = ProcessType.normal;
        }
    }

    public abstract class SystemProcess : Process
    {
        public bool topMost;
        public bool onTaskBar;

        public SystemProcess(string name) : base(name)
        {
            this.topMost = true;
            this.onTaskBar = false;
            this.type = ProcessType.system;
        }

        public abstract void Update();
        public abstract void Draw();
    }



    public enum Priority
    {
        low,
        normal,
        high,
    }
}
