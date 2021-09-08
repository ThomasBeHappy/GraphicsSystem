using Cosmos.HAL;
using GraphicsSystem.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsSystem.Core
{
    public static class Clock
    {
        public static int GetYear() { return RTC.Year; }
        public static int GetMonth() { return RTC.Month; }
        public static int GetDayOfTheMonth() { return RTC.DayOfTheMonth; }
        public static int GetHour() { return RTC.Hour; }
        public static int GetMinute() { return RTC.Minute; }
        public static int GetSecond() { return RTC.Second; }

        static char[] year = new char[32];
        static char[] month = new char[32];
        static char[] day = new char[32];
        static char[] hour = new char[32];
        static char[] minute = new char[32];
        static char[] second = new char[32];

        static char[] seperator = new char[] { ':', '\0' };
        static char[] seperator2 = new char[] { '/', '\0' };
        static char[] newLine = new char[] { '\n', '\0'};

        static char[] tempString = new char[32];
        static char[] temp2String = new char[32];
        static Font font = new FontMono9x11();

        public static char[] GetClockString()
        {
            InternalString.IntToString(GetYear(), ref year);
            InternalString.IntToString(GetMonth(), ref month);
            InternalString.IntToString(GetDayOfTheMonth(), ref day);
            InternalString.IntToString(GetHour(), ref hour);
            InternalString.IntToString(GetMinute(), ref minute);
            InternalString.IntToString(GetSecond(), ref second);

            InternalString.combineString(ref day, ref seperator2, ref tempString);

            InternalString.combineString(ref tempString, ref month, ref temp2String);
            InternalString.combineString(ref temp2String, ref seperator2, ref tempString);

            InternalString.combineString(ref tempString, ref year, ref temp2String);
            InternalString.combineString(ref temp2String, ref newLine, ref tempString);

            InternalString.combineString(ref tempString, ref hour, ref temp2String);
            InternalString.combineString(ref temp2String, ref seperator, ref tempString);

            InternalString.combineString(ref tempString, ref minute, ref temp2String);

            return temp2String;
        }


        public static void Draw()
        {
            Graphics.DrawString(Graphics.width - 80, Graphics.height - 40, font, GetClockString(), Color.white);
        }
    }
}
