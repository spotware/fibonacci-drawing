using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cAlgo.Patterns
{
    public class FibonacciRetracementPattern : PatternBase
    {
        private ChartTrendLine _mainLine;

        private readonly IOrderedEnumerable<FibonacciRetracementLevel> _levels;

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
            if (updatedChartObject.ObjectType != ChartObjectType.TrendLine) return;

            var trendLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine).Cast<ChartTrendLine>().ToArray();

            var mainFan = trendLines.FirstOrDefault(iLine => iLine.Name.IndexOf("1x1", StringComparison.OrdinalIgnoreCase) > -1);

            var sideFans = trendLines.Where(iLine => iLine != mainFan).ToDictionary(iLine => iLine.Name.Split('_').Last());
        }

        protected override void OnDrawingStopped()
        {
            _mainLine = null;
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

            foreach (var level in _levels)
            {
                var levelAmount = level.Percent == 0 ? 0 : verticalDelta * level.Percent;

                var levelLineName = GetObjectName(string.Format("LevelLine_{0}", level.Percent));

                var price = mainLine.Y2 > mainLine.Y1 ? mainLine.Y2 - levelAmount : mainLine.Y2 + levelAmount;

                Chart.DrawTrendLine(levelLineName, _mainLine.Time1, price, _mainLine.Time2, price, level.Color, level.Thickness, level.Style);

                var levelRectangleName = GetObjectName(string.Format("LevelRectangle_{0}", level.Percent));

                if (double.IsNaN(previousLevelPrice))
                {
                    previousLevelPrice = price;

                    continue;
                }

                var rectangle = Chart.DrawRectangle(levelRectangleName, _mainLine.Time1, previousLevelPrice, _mainLine.Time2, price, level.FillColor, 0);

                rectangle.IsFilled = true;

                previousLevelPrice = price;
            }
        }

        protected override void DrawLabels()
        {
            if (_mainLine == null) return;

            //DrawLabels(_mainFan, _sideFans, Id);
        }

        private void DrawLabels(ChartTrendLine mainFan, Dictionary<string, ChartTrendLine> sideFans, long id)
        {
            DrawLabelText("1/1", mainFan.Time2, mainFan.Y2, id, fontSize: 10);

            DrawLabelText("1/2", sideFans["1x2"].Time2, sideFans["1x2"].Y2, id, fontSize: 10);
            DrawLabelText("1/3", sideFans["1x3"].Time2, sideFans["1x3"].Y2, id, fontSize: 10);
            DrawLabelText("1/4", sideFans["1x4"].Time2, sideFans["1x4"].Y2, id, fontSize: 10);
            DrawLabelText("1/8", sideFans["1x8"].Time2, sideFans["1x8"].Y2, id, fontSize: 10);

            DrawLabelText("2/1", sideFans["2x1"].Time2, sideFans["2x1"].Y2, id, fontSize: 10);
            DrawLabelText("3/1", sideFans["3x1"].Time2, sideFans["3x1"].Y2, id, fontSize: 10);
            DrawLabelText("4/1", sideFans["4x1"].Time2, sideFans["4x1"].Y2, id, fontSize: 10);
            DrawLabelText("8/1", sideFans["8x1"].Time2, sideFans["8x1"].Y2, id, fontSize: 10);
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var trendLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine).Cast<ChartTrendLine>().ToArray();

            if (trendLines == null) return;

            var mainFan = trendLines.FirstOrDefault(iLine => iLine.Name.IndexOf("1x1", StringComparison.OrdinalIgnoreCase) > -1);

            if (mainFan == null) return;

            var sideFans = trendLines.Where(iLine => iLine != mainFan).ToDictionary(iLine => iLine.Name.Split('_').Last());

            if (labels.Length == 0)
            {
                DrawLabels(mainFan, sideFans, id);

                return;
            }

            foreach (var label in labels)
            {
                ChartTrendLine line;

                if (label.Text.Equals("1/1", StringComparison.OrdinalIgnoreCase))
                {
                    line = mainFan;
                }
                else
                {
                    var keyName = label.Text[0] == '1' ? string.Format("1x{0}", label.Text[2]) : string.Format("{0}x1", label.Text[0]);

                    if (!sideFans.TryGetValue(keyName, out line)) continue;
                }

                label.Time = line.Time2;
                label.Y = line.Y2;
            }
        }
    }
}