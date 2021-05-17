using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class HeadAndShouldersPattern : PatternBase
    {
        private ChartTriangle _leftTriangle;
        private ChartTriangle _rightTriangle;
        private ChartTriangle _headTriangle;

        public HeadAndShouldersPattern(PatternConfig config) : base("Head and Shoulders", config)
        {
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject, ChartObject[] patternObjects)
        {
            var otherTriangle = updatedChartObject as ChartTriangle;

            if (otherTriangle == null) return;

            var chartObjects = Chart.Objects.ToArray();

            var objectNameId = string.Format("{0}_{1}", ObjectName, id);

            foreach (var chartObject in chartObjects)
            {
                if (chartObject == updatedChartObject
                    || chartObject.ObjectType != ChartObjectType.Triangle
                    || !chartObject.Name.StartsWith(objectNameId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var triangle = chartObject as ChartTriangle;

                if ((triangle.Name.EndsWith("Left", StringComparison.InvariantCultureIgnoreCase)
                    && otherTriangle.Name.EndsWith("Head", StringComparison.InvariantCultureIgnoreCase))
                    || (triangle.Name.EndsWith("Head", StringComparison.InvariantCultureIgnoreCase)
                    && otherTriangle.Name.EndsWith("Right", StringComparison.InvariantCultureIgnoreCase)))
                {
                    triangle.Time3 = otherTriangle.Time1;
                    triangle.Y3 = otherTriangle.Y1;
                }
                else if ((triangle.Name.EndsWith("Head", StringComparison.InvariantCultureIgnoreCase)
                    && otherTriangle.Name.EndsWith("Left", StringComparison.InvariantCultureIgnoreCase))
                    || (triangle.Name.EndsWith("Right", StringComparison.InvariantCultureIgnoreCase)
                    && otherTriangle.Name.EndsWith("Head", StringComparison.InvariantCultureIgnoreCase)))
                {
                    triangle.Time1 = otherTriangle.Time3;
                    triangle.Y1 = otherTriangle.Y3;
                }
            }
        }

        protected override void OnDrawingStopped()
        {
            _leftTriangle = null;
            _rightTriangle = null;
            _headTriangle = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 7)
            {
                StopDrawing();

                return;
            }

            if (_leftTriangle == null)
            {
                var name = GetObjectName("Left");

                DrawTriangle(obj, name, ref _leftTriangle);
            }
            else if (_headTriangle == null && MouseUpNumber == 3)
            {
                var name = GetObjectName("Head");

                DrawTriangle(obj, name, ref _headTriangle);
            }
            else if (_rightTriangle == null && MouseUpNumber == 5)
            {
                var name = GetObjectName("Right");

                DrawTriangle(obj, name, ref _rightTriangle);
            }
        }

        private void DrawTriangle(ChartMouseEventArgs mouseEventArgs, string name, ref ChartTriangle triangle)
        {
            triangle = Chart.DrawTriangle(name, mouseEventArgs.TimeValue, mouseEventArgs.YValue, mouseEventArgs.TimeValue,
                mouseEventArgs.YValue, mouseEventArgs.TimeValue, mouseEventArgs.YValue, Color);

            triangle.IsInteractive = true;

            triangle.IsFilled = true;
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 1)
            {
                _leftTriangle.Time2 = obj.TimeValue;
                _leftTriangle.Y2 = obj.YValue;
            }
            else if (MouseUpNumber == 2)
            {
                _leftTriangle.Time3 = obj.TimeValue;
                _leftTriangle.Y3 = obj.YValue;
            }
            else if (MouseUpNumber == 3)
            {
                _headTriangle.Time2 = obj.TimeValue;
                _headTriangle.Y2 = obj.YValue;
            }
            else if (MouseUpNumber == 4)
            {
                _headTriangle.Time3 = obj.TimeValue;
                _headTriangle.Y3 = obj.YValue;
            }
            else if (MouseUpNumber == 5)
            {
                _rightTriangle.Time2 = obj.TimeValue;
                _rightTriangle.Y2 = obj.YValue;
            }
            else if (MouseUpNumber == 6)
            {
                _rightTriangle.Time3 = obj.TimeValue;
                _rightTriangle.Y3 = obj.YValue;
            }
        }

        protected override void DrawLabels()
        {
            if (_leftTriangle == null || _headTriangle == null || _rightTriangle == null) return;

            DrawLabelText("Left", _leftTriangle.Time2, _leftTriangle.Y2);
            DrawLabelText("Head", _headTriangle.Time2, _headTriangle.Y2);
            DrawLabelText("Right", _rightTriangle.Time2, _rightTriangle.Y2);
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var triangles = patternObjects.Select(iObject => iObject as ChartTriangle).ToArray();

            foreach (var label in labels)
            {
                var labelTriangle = triangles.FirstOrDefault(iTriangle => iTriangle.Name.EndsWith(label.Text,
                    StringComparison.OrdinalIgnoreCase));

                if (labelTriangle == null) continue;

                label.Time = labelTriangle.Time2;
                label.Y = labelTriangle.Y2;
            }
        }
    }
}