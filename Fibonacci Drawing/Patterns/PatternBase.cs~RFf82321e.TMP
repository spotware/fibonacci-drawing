using cAlgo.API;
using System;

namespace cAlgo.Patterns
{
    public abstract class PatternBase : IPattern
    {
        private readonly Chart _chart;

        private int _mouseUpNumber;
        private string _name;
        private readonly Color _color;

        public PatternBase(Chart chart, string name, Color color)
        {
            _chart = chart;
            _name = name;
            _color = color;
        }

        public event Action<IPattern> DrawingStarted;

        public event Action<IPattern> DrawingStopped;

        protected Chart Chart
        {
            get { return _chart; }
        }

        protected int MouseUpNumber
        {
            get { return _mouseUpNumber; }
        }

        protected Color Color
        {
            get { return _color; }
        }

        public string Name
        {
            get { return _name; }
        }

        public void StartDrawing()
        {
            _chart.MouseDown += Chart_MouseDown;
            _chart.MouseMove += Chart_MouseMove;
            _chart.MouseUp += Chart_MouseUp;

            _chart.IsScrollingEnabled = false;

            OnDrawingStarted();
        }

        public void StopDrawing()
        {
            _chart.MouseDown -= Chart_MouseDown;
            _chart.MouseMove -= Chart_MouseMove;
            _chart.MouseUp -= Chart_MouseUp;

            _chart.IsScrollingEnabled = true;

            _mouseUpNumber = 0;

            OnDrawingStopped();
        }

        private void OnDrawingStopped()
        {
            var drawingStopped = DrawingStopped;

            if (drawingStopped != null)
            {
                drawingStopped.Invoke(this);
            }
        }

        private void OnDrawingStarted()
        {
            var drawingStarted = DrawingStarted;

            if (drawingStarted != null)
            {
                drawingStarted.Invoke(this);
            }
        }

        private void Chart_MouseMove(ChartMouseEventArgs obj)
        {
            OnMouseMove(obj);
        }

        private void Chart_MouseDown(ChartMouseEventArgs obj)
        {
            OnMouseDown(obj);
        }

        private void Chart_MouseUp(ChartMouseEventArgs obj)
        {
            _mouseUpNumber++;

            OnMouseUp(obj);
        }

        protected virtual void OnMouseMove(ChartMouseEventArgs obj)
        {
        }

        protected virtual void OnMouseDown(ChartMouseEventArgs obj)
        {
        }

        protected virtual void OnMouseUp(ChartMouseEventArgs obj)
        {
        }
    }
}