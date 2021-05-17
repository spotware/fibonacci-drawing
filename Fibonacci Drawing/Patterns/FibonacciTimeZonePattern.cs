using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace cAlgo.Patterns
{
    public class FibonacciTimeZonePattern : PatternBase
    {
        private readonly IEnumerable<FibonacciLevel> _fibonacciLevels;

        private ChartTrendLine _controllerLine;

        public FibonacciTimeZonePattern(PatternConfig config, IEnumerable<FibonacciLevel> fibonacciLevels) : base("Fibonacci Time Zone", config)
        {
            _fibonacciLevels = fibonacciLevels;
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            var controllerLine = updatedChartObject as ChartTrendLine;

            if (controllerLine == null || controllerLine.Name.LastIndexOf("ControllerLine", StringComparison.OrdinalIgnoreCase) < 0) return;

            var startBarIndex = Chart.Bars.GetBarIndex(controllerLine.Time1, Chart.Symbol);

            var barsNumber = controllerLine.GetBarsNumber(Chart.Bars, Chart.Symbol);

            var verticalLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.VerticalLine).Cast<ChartVerticalLine>().ToArray();

            foreach (var verticalLine in verticalLines)
            {
                double lineLevelPercent;

                if (!double.TryParse(verticalLine.Name.Split('_').Last(), NumberStyles.Any, CultureInfo.InvariantCulture, out lineLevelPercent)) continue;

                var level = _fibonacciLevels.FirstOrDefault(iLevel => iLevel.Percent == lineLevelPercent);

                if (level == null) continue;

                var barsAmount = barsNumber * level.Percent;

                var lineBarIndex = controllerLine.Time2 > controllerLine.Time1 ? startBarIndex + barsAmount : startBarIndex - barsAmount;

                verticalLine.Time = Chart.Bars.GetOpenTime(lineBarIndex, Chart.Symbol);
            }
        }

        protected override void OnDrawingStopped()
        {
            _controllerLine = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 2)
            {
                FinishDrawing();

                return;
            }

            var controllerLineName = GetObjectName("ControllerLine");

            _controllerLine = Chart.DrawTrendLine(controllerLineName, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, Color, 1, LineStyle.Dots);

            _controllerLine.IsInteractive = true;
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (_controllerLine == null) return;

            _controllerLine.Time2 = obj.TimeValue;
            _controllerLine.Y2 = obj.YValue;

            var startBarIndex = Chart.Bars.GetBarIndex(_controllerLine.Time1, Chart.Symbol);

            var barsNumber = _controllerLine.GetBarsNumber(Chart.Bars, Chart.Symbol);

            foreach (var level in _fibonacciLevels)
            {
                var levelLineName = GetObjectName(string.Format("Level_{0}", level.Percent.ToString(CultureInfo.InvariantCulture)));

                var barsAmount = barsNumber * level.Percent;

                var lineBarIndex = _controllerLine.Time2 > _controllerLine.Time1 ? startBarIndex + barsAmount : startBarIndex - barsAmount;

                var lineTime = Chart.Bars.GetOpenTime(lineBarIndex, Chart.Symbol);

                var levelLine = Chart.DrawVerticalLine(levelLineName, lineTime, level.LineColor, level.Thickness, level.Style);

                levelLine.IsInteractive = true;

                levelLine.IsLocked = true;
            }
        }

        protected override ChartObject[] GetFrontObjects()
        {
            return new ChartObject[] { _controllerLine };
        }
    }
}