using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Patterns
{
    public class ElliottDoubleComboWavePattern : ElliottWavePatternBase
    {
        public ElliottDoubleComboWavePattern(PatternConfig config) : base("EW WXY", config, 3)
        {
        }

        protected override void DrawLabels()
        {
            if (FirstLine == null || SecondLine == null || ThirdLine == null) return;

            DrawLabels(FirstLine, SecondLine, ThirdLine, Id);
        }

        private void DrawLabels(ChartTrendLine firstLine, ChartTrendLine secondLine, ChartTrendLine thirdLine, long id)
        {
            DrawLabelText("(0)", firstLine.Time1, firstLine.Y1, id);
            DrawLabelText("(W)", secondLine.Time1, secondLine.Y1, id);
            DrawLabelText("(X)", thirdLine.Time1, thirdLine.Y1, id);
            DrawLabelText("(Y)", thirdLine.Time2, thirdLine.Y2, id);
        }

        protected override void UpdateLabels(long id, ChartObject chartObject, ChartText[] labels, ChartObject[] patternObjects)
        {
            var firstLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("FirstLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var secondLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("SecondLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            var thirdLine = patternObjects.FirstOrDefault(iObject => iObject.Name.EndsWith("ThirdLine",
                StringComparison.OrdinalIgnoreCase)) as ChartTrendLine;

            if (firstLine == null || secondLine == null || thirdLine == null) return;

            if (labels.Length == 0)
            {
                DrawLabels(firstLine, secondLine, thirdLine, id);

                return;
            }

            foreach (var label in labels)
            {
                switch (label.Text)
                {
                    case "(0)":
                        label.Time = firstLine.Time1;
                        label.Y = firstLine.Y1;
                        break;

                    case "(W)":
                        label.Time = secondLine.Time1;
                        label.Y = secondLine.Y1;
                        break;

                    case "(X)":
                        label.Time = thirdLine.Time1;
                        label.Y = thirdLine.Y1;
                        break;

                    case "(Y)":
                        label.Time = thirdLine.Time2;
                        label.Y = thirdLine.Y2;
                        break;
                }
            }
        }
    }
}