using cAlgo.API;
using System;

namespace cAlgo.Patterns
{
    public class TrianglePattern : PatternBase
    {
        private ChartTriangle _triangle;

        public TrianglePattern(Chart chart, Color color) : base(chart, "Triangle", color)
        {
            DrawingStopped += args => _triangle = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 3)
            {
                StopDrawing();
            }
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (_triangle == null) return;

            if (MouseUpNumber == 1)
            {
                _triangle.Time2 = obj.TimeValue;
                _triangle.Y2 = obj.YValue;
            }
            else if (MouseUpNumber == 2)
            {
                _triangle.Time3 = obj.TimeValue;
                _triangle.Y3 = obj.YValue;
            }
        }

        protected override void OnMouseDown(ChartMouseEventArgs obj)
        {
            var name = string.Format("Pattern_Triangle_{0}", DateTime.Now.Ticks);

            _triangle = Chart.DrawTriangle(name, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, obj.TimeValue, obj.YValue, Color);

            _triangle.IsInteractive = true;
        }
    }
}