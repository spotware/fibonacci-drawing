using cAlgo.API;
using System;

namespace cAlgo.Helpers
{
    public static class ChartTrendLineExtensions
    {
        public static DateTime GetLineCenterTime(this ChartTrendLine line)
        {
            return line.Time1.AddMilliseconds((line.Time2 - line.Time1).TotalMilliseconds / 2);
        }

        public static double GetLineCenterY(this ChartTrendLine line)
        {
            return line.Y1 + ((line.Y2 - line.Y1) / 2);
        }

        public static double GetVerticalDelta(this ChartTrendLine line)
        {
            return Math.Abs(line.Y1 - line.Y2);
        }
    }
}