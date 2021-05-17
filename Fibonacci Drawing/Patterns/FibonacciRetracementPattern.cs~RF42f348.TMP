using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace cAlgo.Patterns
{
    public class FibonacciRetracementPattern : PatternBase
    {
        private ChartTrendLine _mainLine;

        private readonly IOrderedEnumerable<FibonacciRetracementLevel> _levels;

        private readonly Dictionary<double, ChartTrendLine> _levelLines = new Dictionary<double, ChartTrendLine>();

        public FibonacciRetracementPattern(PatternConfig config, IEnumerable<FibonacciRetracementLevel> levels) : base("Fibonacci Retracement", config)
        {
            if (levels == null)
            {
                throw new ArgumentNullException("levels");
            }

            _levels = levels.OrderBy(iLevel => iLevel.Percent);
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            if (updatedChartObject.ObjectType != ChartObjectType.TrendLine && updatedChartObject.ObjectType != ChartObjectType.Rectangle) return;

            var mainLine = patternObjects.FirstOrDefault(iObject => iObject.ObjectType == ChartObjectType.TrendLine && iObject.Name.EndsWith("MainLine", StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            if (mainLine == null) return;

            var levelLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine && iObject != mainLine)
                .Cast<ChartTrendLine>()
                .ToDictionary(trendLine => double.Parse(trendLine.Name.Split('_').Last(), CultureInfo.InvariantCulture))
                .OrderBy(iLevelLine => iLevelLine.Key);

            if (levelLines == null || !levelLines.Any()) return;

            var levelRectangles = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.Rectangle)
                .Cast<ChartRectangle>()
                .ToDictionary(trendLine => double.Parse(trendLine.Name.Split('_').Last(), CultureInfo.InvariantCulture));

            if (levelRectangles == null || levelRectangles.Count == 0) return;

            UpdatePattern(mainLine, levelLines, levelRectangles);
        }

        private void UpdatePattern(ChartTrendLine mainLine, IOrderedEnumerable<KeyValuePair<double, ChartTrendLine>> levelLines, Dictionary<double, ChartRectangle> levelRectangles)
        {
            var verticalDelta = mainLine.GetVerticalDelta();

            var previousLevelPrice = double.NaN;

            var startTime = mainLine.GetStartTime();
            var endTime = mainLine.GetEndTime();

            foreach (var levelLine in levelLines)
            {
                var percent = levelLine.Key;

                var levelAmount = percent == 0 ? 0 : verticalDelta * percent;

                var price = mainLine.Y2 > mainLine.Y1 ? mainLine.Y2 - levelAmount : mainLine.Y2 + levelAmount;

                levelLine.Value.Time1 = startTime;
                levelLine.Value.Time2 = endTime;

                levelLine.Value.Y1 = price;
                levelLine.Value.Y2 = price;

                if (double.IsNaN(previousLevelPrice))
                {
                    previousLevelPrice = price;

                    continue;
                }

                ChartRectangle levelRectangle;

                if (!levelRectangles.TryGetValue(percent, out levelRectangle)) continue;

                levelRectangle.Time1 = startTime;
                levelRectangle.Time2 = endTime;

                levelRectangle.Y1 = previousLevelPrice;
                levelRectangle.Y2 = price;

                previousLevelPrice = price;
            }
        }

        protected override void OnDrawingStopped()
        {
            _mainLine = null;

            _levelLines.Clear();
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 2)
            {
                FinishDrawing();

                return;
            }

            if (_mainLine == null)
            {
                var name = GetObjectName("MainLine");

                _mainLine = Chart.DrawTrendLine(name, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, Color, 1, LineStyle.Dots);

                _mainLine.IsInteractive = true;
            }
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber > 1 || _mainLine == null) return;

            _mainLine.Time2 = obj.TimeValue;
            _mainLine.Y2 = obj.YValue;

            DrawLevels(_mainLine);
        }

        private void DrawLevels(ChartTrendLine mainLine)
        {
            var verticalDelta = mainLine.GetVerticalDelta();

            var previousLevelPrice = double.NaN;

            var startTime = mainLine.GetStartTime();
            var endTime = mainLine.GetEndTime();

            foreach (var level in _levels)
            {
                var levelAmount = level.Percent == 0 ? 0 : verticalDelta * level.Percent;

                var levelLineName = GetObjectName(string.Format("LevelLine_{0}", level.Percent));

                var price = mainLine.Y2 > mainLine.Y1 ? mainLine.Y2 - levelAmount : mainLine.Y2 + levelAmount;

                var levelLine = Chart.DrawTrendLine(levelLineName, startTime, price, endTime, price, level.LineColor, level.Thickness, level.Style);

                levelLine.IsInteractive = true;
                levelLine.IsLocked = true;

                _levelLines[level.Percent] = levelLine;

                var levelRectangleName = GetObjectName(string.Format("LevelRectangle_{0}", level.Percent));

                if (double.IsNaN(previousLevelPrice))
                {
                    previousLevelPrice = price;

                    continue;
                }

                var rectangle = Chart.DrawRectangle(levelRectangleName, startTime, previousLevelPrice, endTime, price, level.FillColor, 0);

                rectangle.IsFilled = true;

                rectangle.IsInteractive = true;
                rectangle.IsLocked = true;

                previousLevelPrice = price;
            }

            mainLine.ZIndex = Chart.Objects.Count - 1;
        }

        protected override void DrawLabels()
        {
            DrawLabels(_levelLines, Id);
        }

        private void DrawLabels(Dictionary<double, ChartTrendLine> levelLines, long id)
        {
            foreach (var levelLine in levelLines)
            {
                var text = string.Format("{0} ({1})", levelLine.Key, Math.Round(levelLine.Value.Y1, Chart.Symbol.Digits));

                DrawLabelText(text, levelLine.Value.GetStartTime(), levelLine.Value.Y1, id, objectNameKey: levelLine.Key.ToString(CultureInfo.InvariantCulture));
            }
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var levelLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine && !iObject.Name.EndsWith("MainLine", StringComparison.OrdinalIgnoreCase))
                .Cast<ChartTrendLine>()
                .ToDictionary(trendLine => double.Parse(trendLine.Name.Split('_').Last(), CultureInfo.InvariantCulture));

            if (levelLines == null || levelLines.Count == 0) return;

            if (labels.Length == 0)
            {
                DrawLabels(levelLines, id);

                return;
            }

            foreach (var label in labels)
            {
                var percent = double.Parse(label.Name.Split('_').Last(), CultureInfo.InvariantCulture);

                ChartTrendLine levelLine;

                if (!levelLines.TryGetValue(percent, out levelLine)) continue;

                label.Text = string.Format("{0} ({1})", percent, Math.Round(levelLine.Y1, Chart.Symbol.Digits));
                label.Time = levelLine.GetStartTime();
                label.Y = levelLine.Y1;
            }
        }
    }
}