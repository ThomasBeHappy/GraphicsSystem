using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Types
{
    public static class InternalString
    {
        public static void combineString(ref char[] s1, ref char[] s2, ref char[] s3)
        {
            int i = 0;

            for (i = 0; i < s1.Length; i++)
            {
                if (s1[i] == '\0')
                {
                    break;
                }
                s3[i] = s1[i];
            }

            for (int x = 0; x < s2.Length; x++)
            {
                s3[i++] = s2[x];
                if (s2[x] == '\0')
                {
                    break;
                }
            }
        }

        private static char[] xChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static void IntToString(int aValue, ref char[] xResult)
        {
            if (aValue == 0)
            {
                xResult[0] = '0';
                xResult[1] = '\0';
            }
            else
            {
                int pos = 0;
                if (aValue < 0)
                {
                    xResult[pos++] = '-';
                }
                int xValue = aValue;
                if (aValue < 0)
                {
                    xValue *= -1;
                }
                while (xValue > 0)
                {
                    int xValue2 = xValue % 10;
                    xResult[pos++] = xChars[xValue2];
                    xValue /= 10;
                }
                for (int x = 0; x < pos - 1; x++)
                {
                    char temp = xResult[x];
                    xResult[x] = xResult[pos - 1 - x];
                    xResult[pos - 1 - x] = temp;
                }
                xResult[pos++] = '\0';
            }
        }
    }
}
