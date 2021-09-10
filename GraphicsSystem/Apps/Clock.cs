using Cosmos.HAL;
using Cosmos.System.Graphics;
using GraphicsSystem.Core;
using GraphicsSystem.Types;
using GraphicsSystem.Data;
using Point = GraphicsSystem.Types.Point;

namespace GraphicsSystem.Apps
{
    public class Clock : Window
    {
        static char[] name = new char[] { 'C', 'l', 'o', 'c', 'k', '\0' };
        static char[] number = new char[32];
        FontMono9x11 font = new FontMono9x11();

        static Bitmap icon = ProgramBitmaps.bitmap;

        public Clock(uint x, uint y, uint width, uint height) : base(name, x, y, width, height, icon)
        {
        }

        public override void Update()
        {
            Graphics.DrawAngle((int)(x + width / 2), (int)(y + height / 2), RTC.Hour * 30, 40, Color.white);
            Graphics.DrawAngle((int)(x + width / 2), (int)(y + height / 2), RTC.Minute * 6, 50, Color.white);
            Graphics.DrawAngle((int)(x + width / 2), (int)(y + height / 2), RTC.Second * 6, 60, Color.red);

            for (int i = 1; i <= 12; i++)
            {
                InternalString.IntToString(i, ref number);
                GetTextLocation(i * 30, 60);
                Graphics.DrawString((uint)(x + (width / 2) + point.x - ((font.characterWidth * (1 + i/10))) / 2), (uint)(y + (height / 2) - point.y - (font.characterHeight / 2)), font, number, Color.gold);
            }
        }

        double angleY;
        double angleX;
        Point point = new Point(0,0);
        private void GetTextLocation(int angle, int radius)
        {
            angleY = radius * System.Math.Cos(System.Math.PI * 2 * angle / 360);
            angleX = radius * System.Math.Sin(System.Math.PI * 2 * angle / 360);
            point.x = (uint)(System.Math.Round(angleX * 100) / 100);
            point.y = (uint)(System.Math.Round(angleY * 100) / 100);
        }
    }
}
