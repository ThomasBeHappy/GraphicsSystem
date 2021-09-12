using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Core
{
    public unsafe static class FastReplacer
    {
        public unsafe static void Replace(uint* data, uint oldValue, uint* buffer, int size)
        {
            int xBlocksNum;
            int xByteRemaining;
            const int xBlockSize = 128;

            xBlocksNum = System.Math.DivRem(size, xBlockSize, out xByteRemaining);

            for (int i = 0; i < xByteRemaining; i++)
            {
                if (*(data + i) == oldValue)
                {
                    *(data + i) = *(buffer + i);
                }
            }
        }
    }
}
