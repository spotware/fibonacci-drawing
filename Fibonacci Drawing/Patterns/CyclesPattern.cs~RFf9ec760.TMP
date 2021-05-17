using cAlgo.API;

namespace cAlgo.Patterns
{
    public class CyclesPattern : PatternBase
    {
        private int? _mouseDownBarIndex;
        private readonly int _number;

        public CyclesPattern(PatternConfig config, int number) : base("Cycles", config)
        {
            _number = number;
        }

        protected override void OnDrawingStopped()
        {
            _mouseDownBarIndex = null;
        }

        protected override void OnMouseUp(ChartMouseEventArgs obj)
        {
            if (MouseUpNumber == 2) FinishDrawing();
        }

        protected override void OnMouseMove(ChartMouseEventArgs obj)
        {
            if (!_mouseDownBarIndex.HasValue) return;

            var mouseMoveBarIndex = (int)obj.BarIndex;

            var diff = mouseMoveBarIndex - _mouseDownBarIndex.Value;

            for (int i = 0; i < _number; i++)
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