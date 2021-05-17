using cAlgo.API;
using cAlgo.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace cAlgo.Patterns
{
    public class FibonacciSpeedResistanceFanPattern : FanPatternBase
    {
        private ChartRectangle _rectangle;

        private readonly Dictionary<double, ChartTrendLine> _horizontalTrendLines = new Dictionary<double, ChartTrendLine>();
        private readonly Dictionary<double, ChartTrendLine> _verticalTrendLines = new Dictionary<double, ChartTrendLine>();

        private ChartTrendLine _extendedHorizontalLine;
        private ChartTrendLine _extendedVerticalLine;

        private readonly FibonacciSpeedResistanceFanSettings _settings;

        public FibonacciSpeedResistanceFanPattern(PatternConfig config, FibonacciSpeedResistanceFanSettings settings) : base("Fibonacci Speed Resistance Fan", config, settings.SideFanSettings, settings.MainFanSettings)
        {
            _settings = settings;
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            base.OnPatternChartObjectsUpdated(id, updatedChartObject, patternObjects);

            var rectangle = patternObjects.FirstOrDefault(iObject => iObject.ObjectType == ChartObjectType.Rectangle) as ChartRectangle;

            if (rectangle == null) return;

            var trendLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine).Cast<ChartTrendLine>();

            var mainFan = trendLines.FirstOrDefault(iLine => iLine.Name.IndexOf("MainFan", StringComparison.OrdinalIgnoreCase) > -1);

            if (mainFan == null) return;

            rectangle.Time1 = mainFan.GetStartTime();
            rectangle.Time2 = mainFan.GetEndTime();

            rectangle.Y1 = mainFan.GetTopPrice();
            rectangle.Y2 = mainFan.GetBottomPrice();

            var horizontalLines = trendLines.Where(iTrendLine => iTrendLine.Name.LastIndexOf("HorizontalLine", StringComparison.OrdinalIgnoreCase) > -1).ToDictionary(iLine => double.Parse(iLine.Name.Split('_').Last(), CultureInfo.InvariantCulture));

            DrawOrUpdateHorizontalLines(mainFan, horizontalLines);

            var verticalLines = trendLines.Where(iTrendLine => iTrendLine.Name.LastIndexOf("VerticalLine", StringComparison.OrdinalIgnoreCase) > -1).ToDictionary(iLine => double.Parse(iLine.Name.Split('_').Last(), CultureInfo.InvariantCulture));

            DrawOrUpdateVerticalLines(mainFan, verticalLines);

            var extendedHorizontalLine = trendLines.FirstOrDefault(iTrendLine => iTrendLine.Name.IndexOf("HorizontalExtendedLine", StringComparison.OrdinalIgnoreCase) > -1);
            var extendedVerticalLine = trendLines.FirstOrDefault(iTrendLine => iTrendLine.Name.IndexOf("VerticalExtendedLine", StringComparison.OrdinalIgnoreCase) > -1);

            if (extendedHorizontalLine != null && extendedVerticalLine != null)
            {
                DrawOrUpdateExtendedSideLines(mainFan, ref extendedHorizontalLine, ref extendedVerticalLine);
            }
        }

        protected override void OnDrawingStopped()
        {
            base.OnDrawingStopped();

            _rectangle = null;

            _extendedHorizontalLine = null;
            _extendedVerticalLine = null;

            _horizontalTrendLines.Clear();
            _verticalTrendLines.Clear();
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (_rectangle == null)
            {
                var name = GetObjectName("Rectangle");

                _rectangle = Chart.DrawRectangle(name, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, _settings.RectangleColor, _settings.RectangleThickness, _settings.RectangleStyle);

                _rectangle.IsInteractive = true;
                _rectangle.IsLocked = true;
            }

            base.OnMouseUp(obj);
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (_rectangle != null)
            {
                _rectangle.Time2 = obj.TimeValue;
                _rectangle.Y2 = obj.YValue;

                DrawOrUpdateHorizontalLines(MainFanLine, _horizontalTrendLines);

                DrawOrUpdateVerticalLines(MainFanLine, _verticalTrendLines);

                DrawOrUpdateExtendedSideLines(MainFanLine, ref _extendedHorizontalLine, ref _extendedVerticalLine);
            }

            base.OnMouseMove(obj);
        }

        protected override void UpdateSideFans(ChartTrendLine mainFan, Dictionary<double, ChartTrendLine> sideFans)
        {
            var startBarIndex = mainFan.GetStartBarIndex(Chart.Bars, Chart.Symbol);
            var endBarIndex = mainFan.GetEndBarIndex(Chart.Bars, Chart.Symbol);

            var barsNumber = mainFan.GetBarsNumber(Chart.Bars, Chart.Symbol);

            var mainFanPriceDelta = Math.Abs(mainFan.Y2 - mainFan.Y1);

            for (var iFan = 0; iFan < SideFanSettings.Length; iFan++)
            {
                var fanSettings = SideFanSettings[iFan];

                double y2;
                DateTime time2;

                if (fanSettings.Percent < 0)
                {
                    var yAmount = mainFanPriceDelta * Math.Abs(fanSettings.Percent);

                    y2 = mainFan.Y2 > mainFan.Y1 ? mainFan.Y2 - yAmount : mainFan.Y2 + yAmount;

                    time2 = mainFan.Time2;
                }
                else
                {
                    y2 = mainFan.Y2;

                    var barsPercent = barsNumber * fanSettings.Percent;

                    var barIndex = mainFan.Time2 > mainFan.Time1 ? endBarIndex - barsPercent : startBarIndex + barsPercent;

                    time2 = Chart.Bars.GetOpenTime(barIndex, Chart.Symbol);
                }

                ChartTrendLine fanLine;

                if (!sideFans.TryGetValue(fanSettings.Percent, out fanLine)) continue;

                fanLine.Time1 = mainFan.Time1;
                fanLine.Time2 = time2;

                fanLine.Y1 = mainFan.Y1;
                fanLine.Y2 = y2;
            }
        }

        protected override void DrawSideFans(ChartTrendLine mainFan)
        {
            var startBarIndex = mainFan.GetStartBarIndex(Chart.Bars, Chart.Symbol);
            var endBarIndex = mainFan.GetEndBarIndex(Chart.Bars, Chart.Symbol);

            var barsNumber = mainFan.GetBarsNumber(Chart.Bars, Chart.Symbol);

            var mainFanPriceDelta = Math.Abs(mainFan.Y2 - mainFan.Y1);

            for (var iFan = 0; iFan < SideFanSettings.Length; iFan++)
            {
                var fanSettings = SideFanSettings[iFan];

                double y2;
                DateTime time2;

                if (fanSettings.Percent < 0)
                {
                    var yAmount = mainFanPriceDelta * Math.Abs(fanSettings.Percent);

                    y2 = mainFan.Y2 > mainFan.Y1 ? mainFan.Y2 - yAmount : mainFan.Y2 + yAmount;

                    time2 = mainFan.Time2;
                }
                else
                {
                    y2 = mainFan.Y2;

                    var barsPercent = barsNumber * fanSettings.Percent;

                    var barIndex = mainFan.Time2 > mainFan.Time1 ? endBarIndex - barsPercent : startBarIndex + barsPercent;

                    time2 = Chart.Bars.GetOpenTime(barIndex, Chart.Symbol);
                }

                var objectName = GetObjectName(string.Format("SideFan_{0}", fanSettings.Percent));

                var trendLine = Chart.DrawTrendLine(objectName, mainFan.Time1, mainFan.Y1, time2, y2, fanSettings.Color, fanSettings.Thickness, fanSettings.Style);

                trendLine.IsInteractive = true;
                trendLine.IsLocked = true;
                trendLine.ExtendToInfinity = true;

                SideFanLines[fanSettings.Percent] = trendLine;
            }
        }

        private void DrawOrUpdateHorizontalLines(ChartTrendLine mainFan, Dictionary<double, ChartTrendLine> horizontalLines)
        {
            var startTime = mainFan.GetStartTime();
            var endTime = mainFan.GetEndTime();

            var verticalDelta = mainFan.GetPriceDelta();

            for (int i = 0; i < _settings.SideFanSettings.Length; i++)
            {
                var fanSettings = _settings.SideFanSettings[i];

                if (fanSettings.Percent > 0) continue;

                var absolutePercent = Math.Abs(fanSettings.Percent);

                var lineLevel = absolutePercent * verticalDelta;

                var level = mainFan.Y2 > mainFan.Y1 ? mainFan.Y2 - lineLevel : mainFan.Y2 + lineLevel;

                ChartTrendLine line;

                if (horizontalLines.TryGetValue(absolutePercent, out line))
                {
                    line.Time1 = startTime;
                    line.Time2 = endTime;

                    line.Y1 = level;
                    line.Y2 = level;
                }
                else
                {
                    var objectName = GetObjectName(string.Format("HorizontalLine_{0}", absolutePercent.ToString(CultureInfo.InvariantCulture)));

                    line = Chart.DrawTrendLine(objectName, startTime, level, endTime, level, _settings.PriceLevelsColor, _settings.PriceLevelsThickness, _settings.PriceLevelsStyle);

                    line.IsInteractive = true;
                    line.IsLocked = true;

                    horizontalLines.Add(absolutePercent, line);
                }
            }
        }

        private void DrawOrUpdateVerticalLines(ChartTrendLine mainFan, Dictionary<double, ChartTrendLine> verticalLines)
        {
            var startBarIndex = mainFan.GetStartBarIndex(Chart.Bars, Chart.Symbol);
            var endBarIndex = mainFan.GetEndBarIndex(Chart.Bars, Chart.Symbol);

            var barsNumber = mainFan.GetBarsNumber(Chart.Bars, Chart.Symbol);

            var rectangleEndTime = mainFan.GetEndTime();

            for (int i = 0; i < _settings.SideFanSettings.Length; i++)
            {
                var fanSettings = _settings.SideFanSettings[i];

                if (fanSettings.Percent < 0) continue;

                var lineLevel = fanSettings.Percent * barsNumber;

                var barIndex = mainFan.Time1 > mainFan.Time2 ? startBarIndex + lineLevel : endBarIndex - lineLevel;

                var time = Chart.Bars.GetOpenTime(barIndex, Chart.Symbol);

                if (time > rectangleEndTime)
                {
                    time = rectangleEndTime;
                }

                ChartTrendLine line;

                if (verticalLines.TryGetValue(fanSettings.Percent, out line))
                {
                    line.Time1 = time;
                    line.Time2 = time;

                    line.Y1 = mainFan.Y1;
                    line.Y2 = mainFan.Y2;
                }
                else
                {
                    var objectName = GetObjectName(string.Format("VerticalLine_{0}", fanSettings.Percent.ToString(CultureInfo.InvariantCulture)));

                    line = Chart.DrawTrendLine(objectName, time, mainFan.Y1, time, mainFan.Y2, _settings.TimeLevelsColor, _settings.TimeLevelsThickness, _settings.TimeLevelsStyle);

                    line.IsInteractive = true;
                    line.IsLocked = true;

                    verticalLines.Add(fanSettings.Percent, line);
                }
            }
        }

        private void DrawOrUpdateExtendedSideLines(ChartTrendLine mainFanLine, ref ChartTrendLine horizontalLine, ref ChartTrendLine verticalLine)
        {
            if (mainFanLine == null) return;

            var time1 = mainFanLine.Time1;
            var time2 = mainFanLine.Time2;

            var y1 = mainFanLine.Y1;
            var y2 = mainFanLine.Y2;

            var timeDelta = mainFanLine.GetTimeDelta();

            var horizontalLineTime2 = time2 > time1 ? time2.Add(timeDelta) : time2.Add(-timeDelta);

            if (horizontalLine == null)
            {
                var name = GetObjectName("HorizontalExtendedLine");

                horizontalLine = Chart.DrawTrendLine(name, time1, y1, horizontalLineTime2, y1, _settings.ExtendedLinesColor, _settings.ExtendedLinesThickness, _settings.ExtendedLinesStyle);

                horizontalLine.IsInteractive = true;
                horizontalLine.IsLocked = true;
                horizontalLine.ExtendToInfinity = true;
            }
            else
            {
                horizontalLine.Time1 = time1;
                horizontalLine.Time2 = horizontalLineTime2;
                horizontalLine.Y1 = y1;
                horizontalLine.Y2 = y1;
            }

            var priceDelta = mainFanLine.GetPriceDelta();

            var verticalLineY2 = y2 > y1 ? y2 + priceDelta : y2 - priceDelta;

            if (verticalLine == null)
            {
                var name = GetObjectName("VerticalExtendedLine");

                verticalLine = Chart.DrawTrendLine(name, time1, y1, time1, verticalLineY2, _settings.ExtendedLinesColor, _settings.ExtendedLinesThickness, _settings.ExtendedLinesStyle);

                verticalLine.IsInteractive = true;
                verticalLine.IsLocked = true;
                verticalLine.ExtendToInfinity = true;
            }
            else
            {
                verticalLine.Time1 = time1;
                verticalLine.Time2 = time1;
                verticalLine.Y1 = y1;
                verticalLine.Y2 = verticalLineY2;
            }
        }

        protected override void DrawLabels()
        {
            if (MainFanLine == null || _horizontalTrendLines == null || _verticalTrendLines == null) return;

            DrawLabels(MainFanLine, _horizontalTrendLines, _verticalTrendLines, Id);
        }

        private void DrawLabels(ChartTrendLine mainFanLine, Dictionary<double, ChartTrendLine> horizontalLines, Dictionary<double, ChartTrendLine> verticalLines, long id)
        {
            var timeDistance = -TimeSpan.FromHours(Chart.Bars.GetTimeDiff().TotalHours * 2);

            DrawLabelText("1", mainFanLine.Time1, mainFanLine.Y1, id, objectNameKey: "1.0", fontSize: 10, color: _settings.MainFanSettings.Color);
            DrawLabelText("0", mainFanLine.Time1, mainFanLine.Y2, id, objectNameKey: "0.0", fontSize: 10, color: _settings.MainFanSettings.Color);

            DrawLabelText("1", mainFanLine.Time1.Add(timeDistance), mainFanLine.Y1, id, objectNameKey: "1.1", fontSize: 10, color: _settings.MainFanSettings.Color);
            DrawLabelText("0", mainFanLine.Time2.Add(timeDistance), mainFanLine.Y1, id, objectNameKey: "0.1", fontSize: 10, color: _settings.MainFanSettings.Color);

            DrawLabelText("1", mainFanLine.Time2, mainFanLine.Y1, id, objectNameKey: "1.2", fontSize: 10, color: _settings.MainFanSettings.Color);
            DrawLabelText("0", mainFanLine.Time2, mainFanLine.Y2, id, objectNameKey: "0.2", fontSize: 10, color: _settings.MainFanSettings.Color);

            DrawLabelText("1", mainFanLine.Time1.Add(timeDistance), mainFanLine.Y2, id, objectNameKey: "1.3", fontSize: 10, color: _settings.MainFanSettings.Color);
            DrawLabelText("0", mainFanLine.Time2.Add(timeDistance), mainFanLine.Y2, id, objectNameKey: "0.3", fontSize: 10, color: _settings.MainFanSettings.Color);

            foreach (var horizontalLine in horizontalLines)
            {
                var fanSettings = _settings.SideFanSettings.FirstOrDefault(iFanSettings => iFanSettings.Percent == -horizontalLine.Key);

                if (fanSettings == null) continue;

                var text = horizontalLine.Key.ToString();
                var color = fanSettings.Color;

                var firstLabelObjectNameKey = string.Format("Horizontal_0_{0}", horizontalLine.Key);
                var secondLabelObjectNameKey = string.Format("Horizontal_1_{0}", horizontalLine.Key);

                DrawLabelText(text, horizontalLine.Value.Time1, horizontalLine.Value.Y1, id, objectNameKey: firstLabelObjectNameKey, fontSize: 10, color: color);
                DrawLabelText(text, horizontalLine.Value.Time2, horizontalLine.Value.Y2, id, objectNameKey: secondLabelObjectNameKey, fontSize: 10, color: color);
            }

            foreach (var verticalLine in verticalLines)
            {
                var fanSettings = _settings.SideFanSettings.FirstOrDefault(iFanSettings => iFanSettings.Percent == verticalLine.Key);

                if (fanSettings == null) continue;

                var text = verticalLine.Key.ToString();
                var color = fanSettings.Color;

                var firstLabelObjectNameKey = string.Format("Vertical_0_{0}", verticalLine.Key);
                var secondLabelObjectNameKey = string.Format("Vertical_1_{0}", verticalLine.Key);

                DrawLabelText(text, verticalLine.Value.Time1, verticalLine.Value.Y1, id, objectNameKey: firstLabelObjectNameKey, fontSize: 10, color: color);
                DrawLabelText(text, verticalLine.Value.Time2, verticalLine.Value.Y2, id, objectNameKey: secondLabelObjectNameKey, fontSize: 10, color: color);
            }
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var trendLines = patternObjects.Where(iObject => iObject.ObjectType == ChartObjectType.TrendLine).Cast<ChartTrendLine>();

            var horizontalLines = trendLines.Where(iTrendLine => iTrendLine.Name.LastIndexOf("HorizontalLine", StringComparison.OrdinalIgnoreCase) > -1).ToDictionary(iLine => double.Parse(iLine.Name.Split('_').Last(), CultureInfo.InvariantCulture));

            var verticalLines = trendLines.Where(iTrendLine => iTrendLine.Name.LastIndexOf("VerticalLine", StringComparison.OrdinalIgnoreCase) > -1).ToDictionary(iLine => double.Parse(iLine.Name.Split('_').Last(), CultureInfo.InvariantCulture));

            var mainFan = trendLines.FirstOrDefault(iLine => iLine.Name.IndexOf("MainFan", StringComparison.OrdinalIgnoreCase) > -1);

            var sideFans = trendLines.Where(iLine => iLine.Name.IndexOf("SideFan", StringComparison.OrdinalIgnoreCase) > -1).ToDictionary(iLine => double.Parse(iLine.Name.Split('_').Last(), CultureInfo.InvariantCulture));

            if (horizontalLines == null || verticalLines == null || mainFan == null || sideFans == null) return;

            if (labels.Length == 0)
            {
                DrawLabels(mainFan, horizontalLines, verticalLines, id);

                return;
            }

            foreach (var horizontalLine in horizontalLines)
            {
                ChartTrendLine sideFanLine;

                if (!sideFans.TryGetValue(horizontalLine.Key, out sideFanLine)) continue;

                var firstLabelObjectNameKey = string.Format("Horizontal_0_{0}", horizontalLine.Key);

                var firstLabel = labels.FirstOrDefault(iLabel => iLabel.Name.LastIndexOf(firstLabelObjectNameKey, StringComparison.OrdinalIgnoreCase) > -1);

                if (firstLabel == null) continue;

                firstLabel.Time = horizontalLine.Value.Time1;
                firstLabel.Y = horizontalLine.Value.Y1;
                firstLabel.Color = sideFanLine.Color;

                var secondLabelObjectNameKey = string.Format("Horizontal_1_{0}", horizontalLine.Key);

                var secondLabel = labels.FirstOrDefault(iLabel => iLabel.Name.LastIndexOf(secondLabelObjectNameKey, StringComparison.OrdinalIgnoreCase) > -1);

                if (secondLabel == null) continue;

                secondLabel.Time = horizontalLine.Value.Time2;
                secondLabel.Y = horizontalLine.Value.Y2;
                secondLabel.Color = sideFanLine.Color;
            }

            foreach (var verticalLine in verticalLines)
            {
                ChartTrendLine sideFanLine;

                if (!sideFans.TryGetValue(verticalLine.Key, out sideFanLine)) continue;

                var firstLabelObjectNameKey = string.Format("Vertical_0_{0}", verticalLine.Key);

                var firstLabel = labels.FirstOrDefault(iLabel => iLabel.Name.LastIndexOf(firstLabelObjectNameKey, StringComparison.OrdinalIgnoreCase) > -1);

                if (firstLabel == null) continue;

                firstLabel.Time = verticalLine.Value.Time1;
                firstLabel.Y = verticalLine.Value.Y1;
                firstLabel.Color = sideFanLine.Color;

                var secondLabelObjectNameKey = string.Format("Vertical_1_{0}", verticalLine.Key);

                var secondLabel = labels.FirstOrDefault(iLabel => iLabel.Name.LastIndexOf(secondLabelObjectNameKey, StringComparison.OrdinalIgnoreCase) > -1);

                if (secondLabel == null) continue;

                secondLabel.Time = verticalLine.Value.Time2;
                secondLabel.Y = verticalLine.Value.Y2;
                secondLabel.Color = sideFanLine.Color;
            }

            foreach (var label in labels)
            {
                if (label.Name.LastIndexOf("Label_0", StringComparison.OrdinalIgnoreCase) < 0 && label.Name.LastIndexOf("Label_1", StringComparison.OrdinalIgnoreCase) < 0) continue;

                label.Color = mainFan.Color;

                var labelKey = label.Name.Split('_').Last();

                var timeDistance = -TimeSpan.FromHours(Chart.Bars.GetTimeDiff().TotalHours * 2);

                switch (labelKey)
                {
                    case "1.0":
                        label.Time = mainFan.Time1;
                        label.Y = mainFan.Y1;
                        break;

                    case "0.0":
                        label.Time = mainFan.Time1;
                        label.Y = mainFan.Y2;
                        break;

                    case "1.1":
                        label.Time = mainFan.Time1.Add(timeDistance);
                        label.Y = mainFan.Y1;
                        break;

                    case "0.1":
                        label.Time = mainFan.Time2.Add(timeDistance);
                        label.Y = mainFan.Y1;
                        break;

                    case "1.2":
                        label.Time = mainFan.Time2;
                        label.Y = mainFan.Y1;
                        break;

                    case "0.2":
                        label.Time = mainFan.Time2;
                        label.Y = mainFan.Y2;
                        break;

                    case "1.3":
                        label.Time = mainFan.Time1.Add(timeDistance);
                        label.Y = mainFan.Y2;
                        break;

                    case "0.3":
                        label.Time = mainFan.Time2.Add(timeDistance);
                        label.Y = mainFan.Y2;
                        break;
                }
            }
        }

        protected override void UpdateLabelsStyle(ChartText[] labels, ChartText updatedLabel)
        {
            foreach (var label in labels)
            {
                label.FontSize = updatedLabel.FontSize;
                label.IsBold = updatedLabel.IsBold;
                label.IsItalic = updatedLabel.IsItalic;
                label.IsLocked = updatedLabel.IsLocked;
                label.IsUnderlined = updatedLabel.IsUnderlined;
                label.HorizontalAlignment = updatedLabel.HorizontalAlignment;
                label.VerticalAlignment = updatedLabel.VerticalAlignment;
            }
        }
    }
}