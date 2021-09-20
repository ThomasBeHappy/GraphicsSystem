using Cosmos.HAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Math
{
    public static class Time
    {
        public static float TimeDelta(int fps) { return 1f / fps; }
    }
}
