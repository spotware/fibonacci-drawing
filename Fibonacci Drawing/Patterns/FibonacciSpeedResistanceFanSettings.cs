using cAlgo.API;

namespace cAlgo.Patterns
{
    public class FibonacciSpeedResistanceFanSettings
    {
        public int RectangleThickness { get; set; }

        public LineStyle RectangleStyle { get; set; }

        public Color RectangleColor { get; set; }

        public int PriceLevelsThickness { get; set; }

        public LineStyle PriceLevelsStyle { get; set; }

        public Color PriceLevelsColor { get; set; }

        public int TimeLevelsThickness { get; set; }

        public LineStyle TimeLevelsStyle { get; set; }

        public Color TimeLevelsColor { get; set; }

        public int ExtendedLinesThickness { get; set; }

        public LineStyle ExtendedLinesStyle { get; set; }

        public Color ExtendedLinesColor { get; set; }

        public SideFanSettings[] SideFanSettings { get; set; }

        public FanSettings MainFanSettings { get; set; }

        public bool ShowPriceLevels { get; set; }

        public bool ShowTimeLevels { get; set; }
    }
}