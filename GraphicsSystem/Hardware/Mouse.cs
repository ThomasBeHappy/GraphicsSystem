using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace GraphicsSystem.Hardware
{
    public static class Mouse
    {
        public static ushort x { get { return (ushort)Sys.MouseManager.X; } }
        public static ushort y { get { return (ushort)Sys.MouseManager.Y; } }
        public static Point position { get { return new Point((ushort)Sys.MouseManager.X, (ushort)Sys.MouseManager.Y); } }
        public static Point positionOld;
        public static bool moving { get; private set; }
    }
}
