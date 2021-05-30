using cAlgo.API;

namespace cAlgo.Patterns
{
    public class FibonacciLevel : PercentLineSettings
    {
        public Color FillColor { get; set; }

        public bool IsFilled { get; set; }

        public bool ExtendToInfinity { get; set; }
    }
}