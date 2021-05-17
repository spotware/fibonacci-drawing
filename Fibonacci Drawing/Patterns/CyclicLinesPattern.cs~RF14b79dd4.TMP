using cAlgo.API;
using System;

namespace cAlgo.Patterns
{
    public class CyclicLinesPattern : PatternBase
    {
        private int? _mouseDownBarIndex;

        private long _id;

        public CyclicLinesPattern(Chart chart, Color color) : base(chart, "Cyclic Lines", color)
        {
            DrawingStopped += args => _mouseDownBarIndex = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 2)
            {
                StopDrawing();
            }
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (!_mouseDownBarIndex.HasValue) return;

            var mouseMoveBarIndex = (int)obj.BarIndex;

            var diff = mouseMoveBarIndex - _mouseDownBarIndex.Value;

            for (int i = 0; i < 100; i++)
            {
                var name = string.Format("{0}_{1}_{2}", ObjectName, _id, i);

                var lineIndex = _mouseDownBarIndex.Value + (diff * i);

                var verticalLine = Chart.DrawVerticalLine(name, lineIndex, Color);

                verticalLine.IsInteractive = true;
            }
        }

        protected override void OnMouseDown(ChartMouseEventArgs obj)
        {
            if (_mouseDownBarIndex.HasValue) return;

            _mouseDownBarIndex = (int)obj.BarIndex;

            _id = DateTime.Now.Ticks;
        }
    }
}