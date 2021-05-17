using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace cAlgo.Patterns
{
    public class TrendBasedFibonacciTimePattern : PatternBase
    {
        private readonly IEnumerable<FibonacciLevel> _fibonacciLevels;

        private ChartTrendLine _mainLine, _distanceLine;

        public TrendBasedFibonacciTimePattern(PatternConfig config, IEnumerable<FibonacciLevel> fibonacciLevels) : base("Trend Based Fibonacci Time", config)
        {
            _fibonacciLevels = fibonacciLevels;
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            if (updatedChartObject.ObjectType != ChartObjectType.TrendLine) return;

            var mainLine = patternObjects.FirstOrDefault(iLine => iLine.Name.LastIndexOf("MainLine", StringComparison.OrdinalIgnoreCase) >= 0) as ChartTrendLine;
            var distanceLine = patternObjects.FirstOrDefault(iLine => iLine.Name.LastIndexOf("DistanceLine", StringComparison.OrdinalIgnoreCase) >= 0) as ChartTrendLine;

            if (mainLine == null || distanceLine == null) return;

            if (updatedChartObject == mainLine)
            {
                distanceLine.Time1 = mainLine.Time2;
                distanceLine.Y1 = mainLine.Y2;
            }
            else if (updatedChartObject == distanceLine)
            {
                mainLine.Time2 = distanceLine.Time1;
                mainLine.Y2 = distanceLine.Y1;
            }

            var verticalLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.VerticalLine).Cast<ChartVerticalLine>().ToArray();

            UpdateFibonacciLevels(mainLine, distanceLine, verticalLines);
        }

        protected override void OnDrawingStopped()
        {
            _mainLine = null;
            _distanceLine = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 1)
            {
                var mainLineName = GetObjectName("MainLine");

                _mainLine = Chart.DrawTrendLine(mainLineName, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, Color, 1, LineStyle.Dots);

                _mainLine.IsInteractive = true;
            }
            else if (MouseUpNumber == 2 && _mainLine != null)
            {
                var distanceLineName = GetObjectName("DistanceLine");

                _distanceLine = Chart.DrawTrendLine(distanceLineName, _mainLine.Time2, _mainLine.Y2, obj.TimeValue, obj.YValue, Color, 1, LineStyle.Dots);

                _distanceLine.IsInteractive = true;
            }
            else
            {
                DrawFibonacciLevels();

                FinishDrawing();
            }
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            ChartTrendLine line = null;

            if (MouseUpNumber == 1)
            {
                line = _mainLine;
            }
            else if (MouseUpNumber == 2)
            {
                line = _distanceLine;
            }

            if (line == null) return;

            line.Time2 = obj.TimeValue;
            line.Y2 = obj.YValue;
        }

        protected override ChartObject[] GetFrontObjects()
        {
            return new ChartObject[] { _mainLine, _distanceLine };
        }

        private void DrawFibonacciLevels()
        {
            var startBarIndex = Chart.Bars.GetBarIndex(_distanceLine.Time2, Chart.Symbol);

            var barsNumber = _mainLine.GetBarsNumber(Chart.Bars, Chart.Symbol);

            foreach (var level in _fibonacciLevels)
            {
                var levelLineName = GetObjectName(string.Format("Level_{0}", level.Percent.ToString(CultureInfo.InvariantCulture)));

                var barsAmount = barsNumber * level.Percent;

                var lineBarIndex = _mainLine.Time2 > _mainLine.Time1 ? startBarIndex + barsAmount : startBarIndex - barsAmount;

                var lineTime = Chart.Bars.GetOpenTime(lineBarIndex, Chart.Symbol);

                var levelLine = Chart.DrawVerticalLine(levelLineName, lineTime, level.LineColor, level.Thickness, level.Style);

                levelLine.IsInteractive = true;

                levelLine.IsLocked = true;
            }
        }

        private void UpdateFibonacciLevels(ChartTrendLine mainLine, ChartTrendLine distanceLine, ChartVerticalLine[] verticalLines)
        {
            var startBarIndex = Chart.Bars.GetBarIndex(distanceLine.Time2, Chart.Symbol);

            var barsNumber = mainLine.GetBarsNumber(Chart.Bars, Chart.Symbol);

            foreach (var verticalLine in verticalLines)
            {
                double lineLevelPercent;

                if (!double.TryParse(verticalLine.Name.Split('_').Last(), NumberStyles.Any, CultureInfo.InvariantCulture, out lineLevelPercent)) continue;

                var level = _fibonacciLevels.FirstOrDefault(iLevel => iLevel.Percent == lineLevelPercent);

                if (level == null) continue;

                var barsAmount = barsNumber * level.Percent;

                var lineBarIndex = mainLine.Time2 > mainLine.Time1 ? startBarIndex + barsAmount : startBarIndex - barsAmount;

                verticalLine.Time = Chart.Bars.GetOpenTime(lineBarIndex, Chart.Symbol);
            }
        }
    }
}