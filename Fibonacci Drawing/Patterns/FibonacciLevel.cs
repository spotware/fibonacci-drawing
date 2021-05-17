using cAlgo.API;

namespace cAlgo.Patterns
{
    public class FibonacciLevel
    {
        public double Percent { get; set; }

        public Color LineColor { get; set; }

        public Color FillColor { get; set; }

        public LineStyle Style { get; set; }

        public int Thickness { get; set; }

        public bool IsFilled { get; set; }
    }
}