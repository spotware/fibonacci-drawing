using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class AbcdPattern : PatternBase
    {
        private ChartTriangle _leftTriangle;
        private ChartTriangle _rightTriangle;

        private long _id;

        public AbcdPattern(Chart chart, Color color) : base(chart, "ABCD", Color.FromArgb(100, color))
        {
        }

        protected override void OnPatternChartObjectsUpdated(long id, ChartObject updatedChartObject)
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

                if (triangle.Name.EndsWith("Left", StringComparison.InvariantCultureIgnoreCase))
                {
                    triangle.Time3 = otherTriangle.Time1;
                    triangle.Y3 = otherTriangle.Y1;
                }
                else
                {
                    triangle.Time1 = otherTriangle.Time3;
                    triangle.Y1 = otherTriangle.Y3;
                }

                triangle.Time2 = otherTriangle.Time2;
                triangle.Y2 = otherTriangle.Y2;
            }
        }

        protected override void OnDrawingStopped()
        {
            _leftTriangle = null;
            _rightTriangle = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 4)
            {
                StopDrawing();

                return;
            }

            if (_leftTriangle == null)
            {
                _id = DateTime.Now.Ticks;

                var name = string.Format("{0}_{1}_Left", ObjectName, _id);

                DrawTriangle(obj, name, ref _leftTriangle);
            }
            else if (_rightTriangle == null && MouseUpNumber == 3)
            {
                var name = string.Format("{0}_{1}_Right", ObjectName, _id);

                DrawTriangle(obj, name, ref _rightTriangle);

                _rightTriangle.Time2 = _leftTriangle.Time2;
                _rightTriangle.Y2 = _leftTriangle.Y2;
                _rightTriangle.Color = Color.FromArgb(70, Color);
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
                _rightTriangle.Time3 = obj.TimeValue;
                _rightTriangle.Y3 = obj.YValue;
            }
        }
    }
}