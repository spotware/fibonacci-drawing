using cAlgo.API;
using System;

namespace cAlgo.Patterns
{
    public class PatternConfig
    {
        public PatternConfig(Chart chart, Color color, bool showLabels, Color labelsColor, bool isLabelsLocked, bool isLabelsStyleLinked)
        {
            Chart = chart;
            Color = color;
            ShowLabels = showLabels;
            LabelsColor = labelsColor;
            IsLabelsLocked = isLabelsLocked;
            IsLabelsStyleLinked = isLabelsStyleLinked;
        }

        public Chart Chart { get; private set; }

        public Color Color { get; private set; }

        public bool ShowLabels { get; private set; }

        public Color LabelsColor { get; private set; }

        public bool IsLabelsLocked { get; private set; }

        public bool IsLabelsStyleLinked { get; private set; }

        public Action<string> Print { get; set; }
    }
}