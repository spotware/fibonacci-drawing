using cAlgo.API;

namespace cAlgo.Patterns
{
    public class CyclicLinesPattern : PatternBase
    {
        private int? _mouseDownBarIndex;

        public CyclicLinesPattern(PatternConfig config) : base("Cyclic Lines", config)
        {
        }

        protected override void OnDrawingStopped()
        {
            _mouseDownBarIndex = null;
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
                var name = GetObjectName(i.ToString());

                var lineIndex = _mouseDownBarIndex.Value + (diff * i);

                var verticalLine = Chart.DrawVerticalLine(name, lineIndex, Color);

                verticalLine.IsInteractive = true;
            }
        }

        protected override void OnMouseDown(ChartMouseEventArgs obj)
        {
            if (_mouseDownBarIndex.HasValue) return;

            _mouseDownBarIndex = (int)obj.BarIndex;
        }
    }
}