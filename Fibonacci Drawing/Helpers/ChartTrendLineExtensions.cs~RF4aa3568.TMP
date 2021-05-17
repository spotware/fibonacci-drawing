using cAlgo.API;
using cAlgo.API.Internals;
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

        public static double GetPriceDelta(this ChartTrendLine line)
        {
            return Math.Abs(line.Y1 - line.Y2);
        }

        public static DateTime GetStartTime(this ChartTrendLine line)
        {
            return line.Time1 > line.Time2 ? line.Time2 : line.Time1;
        }

        public static DateTime GetEndTime(this ChartTrendLine line)
        {
            return line.Time1 > line.Time2 ? line.Time1 : line.Time2;
        }

        public static double GetStartBarIndex(this ChartTrendLine line, Bars bars, Symbol symbol)
        {
            return bars.GetBarIndex(line.GetStartTime(), symbol);
        }

        public static double GetEndBarIndex(this ChartTrendLine line, Bars bars, Symbol symbol)
        {
            return bars.GetBarIndex(line.GetEndTime(), symbol);
        }

        public static double GetTopPrice(this ChartTrendLine line)
        {
            return line.Y2 > line.Y1 ? line.Y2 : line.Y1;
        }

        public static double GetBottomPrice(this ChartTrendLine line)
        {
            return line.Y2 > line.Y1 ? line.Y1 : line.Y2;
        }

        public static double GetBarsNumber(this ChartTrendLine line, Bars bars, Symbol symbol)
        {
            var startX = line.GetStartTime();
            var endX = line.GetEndTime();

            var startBarIndex = bars.GetBarIndex(startX, symbol);
            var endBarIndex = bars.GetBarIndex(endX, symbol);

            return Math.Round(endBarIndex - startBarIndex, 2);
        }

        public static TimeSpan GetTimeDelta(this ChartTrendLine line)
        {
            return line.GetEndTime() - line.GetStartTime();
        }
    }
}