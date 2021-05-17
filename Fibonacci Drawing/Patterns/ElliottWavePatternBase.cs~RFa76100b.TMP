using cAlgo.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cAlgo.Patterns
{
    public abstract class ElliottWavePatternBase : PatternBase
    {
        private ChartTrendLine _firstLine, _secondLine, _thirdLine, _fourthLine, _fifthLine;

        private int _linesNumber;

        private readonly ElliottWaveDegree _degree;

        public ElliottWavePatternBase(string name, PatternConfig config, int linesNumber, ElliottWaveDegree degree) : base(degree.ToString(), config, objectName: string.Format("Pattern_{0}{1}", name.Replace(" ", "").Replace("_", ""), degree))
        {
            if (linesNumber > 5) throw new ArgumentOutOfRangeException("linesNumber");

            _linesNumber = linesNumber;
            _degree = degree;
        }

        protected ElliottWaveDegree Degree
        {
            get { return _degree; }
        }

        protected ChartTrendLine FirstLine
        {
            get { return _firstLine; }
        }

        protected ChartTrendLine SecondLine
        {
            get { return _secondLine; }
        }

        protected ChartTrendLine ThirdLine
        {
            get { return _thirdLine; }
        }

        protected ChartTrendLine FourthLine
        {
            get { return _fourthLine; }
        }

        protected ChartTrendLine FifthLine
        {
            get { return _fifthLine; }
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            var updatedLine = updatedChartObject as ChartTrendLine;

            if (updatedLine == null) return;

            if (updatedLine.Name.EndsWith("FirstLine", StringComparison.OrdinalIgnoreCase) && _linesNumber >= 1)
            {
                UpdateSideLines(updatedLine, patternObjects, null, "SecondLine");
            }
            else if (updatedLine.Name.EndsWith("SecondLine", StringComparison.OrdinalIgnoreCase) && _linesNumber >= 2)
            {
                UpdateSideLines(updatedLine, patternObjects, "FirstLine", "ThirdLine");
            }
            else if (updatedLine.Name.EndsWith("ThirdLine", StringComparison.OrdinalIgnoreCase) && _linesNumber >= 3)
            {
                UpdateSideLines(updatedLine, patternObjects, "SecondLine", "FourthLine");
            }
            else if (updatedLine.Name.EndsWith("FourthLine", StringComparison.OrdinalIgnoreCase) && _linesNumber >= 4)
            {
                UpdateSideLines(updatedLine, patternObjects, "ThirdLine", "FifthLine");
            }
            else if (updatedLine.Name.EndsWith("FifthLine", StringComparison.OrdinalIgnoreCase) && _linesNumber >= 5)
            {
                UpdateSideLines(updatedLine, patternObjects, "FourthLine", null);
            }
        }

        private void UpdateSideLines(ChartTrendLine line, ChartObject[] patternObjects, string leftLineName, string rightLineName)
        {
            if (!string.IsNullOrWhiteSpace(leftLineName))
            {
                var leftLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith(leftLineName,
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

                if (leftLine != null)
                {
                    leftLine.Time2 = line.Time1;
                    leftLine.Y2 = line.Y1;
                }
            }

            if (!string.IsNullOrWhiteSpace(rightLineName))
            {
                var rightLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith(rightLineName,
                    StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

                if (rightLine != null)
                {
                    rightLine.Time1 = line.Time2;
                    rightLine.Y1 = line.Y2;
                }
            }
        }

        protected override void OnDrawingStopped()
        {
            _firstLine = null;
            _secondLine = null;
            _thirdLine = null;
            _fourthLine = null;
            _fifthLine = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == _linesNumber + 1)
            {
                FinishDrawing();

                return;
            }

            if (_firstLine == null && _linesNumber >= 1)
            {
                var name = GetObjectName("FirstLine");

                DrawLine(obj, name, ref _firstLine);
            }
            else if (_secondLine == null && MouseUpNumber == 2 && _linesNumber >= 2)
            {
                var name = GetObjectName("SecondLine");

                DrawLine(obj, name, ref _secondLine);
            }
            else if (_thirdLine == null && MouseUpNumber == 3 && _linesNumber >= 3)
            {
                var name = GetObjectName("ThirdLine");

                DrawLine(obj, name, ref _thirdLine);
            }
            else if (_fourthLine == null && MouseUpNumber == 4 && _linesNumber >= 4)
            {
                var name = GetObjectName("FourthLine");

                DrawLine(obj, name, ref _fourthLine);
            }
            else if (_fifthLine == null && MouseUpNumber == 5 && _linesNumber >= 5)
            {
                var name = GetObjectName("FifthLine");

                DrawLine(obj, name, ref _fifthLine);
            }
        }

        private void DrawLine(ChartMouseEventArgs mouseEventArgs, string name, ref ChartTrendLine line)
        {
            line = Chart.DrawTrendLine(name, mouseEventArgs.TimeValue, mouseEventArgs.YValue, mouseEventArgs.TimeValue,
                mouseEventArgs.YValue, Color);

            line.IsInteractive = true;
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            switch (MouseUpNumber)
            {
                case 1:
                    _firstLine.Time2 = obj.TimeValue;
                    _firstLine.Y2 = obj.YValue;
                    return;

                case 2:
                    if (_secondLine == null) return;

                    _secondLine.Time2 = obj.TimeValue;
                    _secondLine.Y2 = obj.YValue;
                    return;

                case 3:
                    if (_thirdLine == null) return;

                    _thirdLine.Time2 = obj.TimeValue;
                    _thirdLine.Y2 = obj.YValue;
                    return;

                case 4:
                    if (_fourthLine == null) return;

                    _fourthLine.Time2 = obj.TimeValue;
                    _fourthLine.Y2 = obj.YValue;
                    return;

                case 5:
                    if (_fifthLine == null) return;

                    _fifthLine.Time2 = obj.TimeValue;
                    _fifthLine.Y2 = obj.YValue;
                    return;
            }
        }

        protected override ChartObject[] GetFrontObjects()
        {
            var frontObjects = new List<ChartObject>();

            if (_firstLine != null) frontObjects.Add(_firstLine);
            if (_secondLine != null) frontObjects.Add(_secondLine);
            if (_thirdLine != null) frontObjects.Add(_thirdLine);
            if (_fourthLine != null) frontObjects.Add(_fourthLine);
            if (_fifthLine != null) frontObjects.Add(_fifthLine);

            return frontObjects.ToArray();
        }
    }
}