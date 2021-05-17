using cAlgo.API;
using cAlgo.Controls;
using cAlgo.Helpers;
using cAlgo.Patterns;
using System.Collections.Generic;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class FibonacciDrawing : Indicator
    {
        private StackPanel _mainButtonsPanel;

        private StackPanel _groupButtonsPanel;

        private StackPanel _mainPanel;

        private Color _buttonsBackgroundDisableColor;

        private Color _buttonsBackgroundEnableColor;

        private Style _buttonsStyle;

        private readonly List<Button> _buttons = new List<Button>();

        private Button _expandButton;

        #region Patterns color parameters

        [Parameter("Color", DefaultValue = "Red", Group = "Patterns Color")]
        public string PatternsColor { get; set; }

        [Parameter("Alpha", DefaultValue = 100, MinValue = 0, MaxValue = 255, Group = "Patterns Color")]
        public int PatternsColorAlpha { get; set; }

        #endregion Patterns color parameters

        #region Patterns Label parameters

        [Parameter("Show", DefaultValue = true, Group = "Patterns Label")]
        public bool PatternsLabelShow { get; set; }

        [Parameter("Color", DefaultValue = "Yellow", Group = "Patterns Label")]
        public string PatternsLabelColor { get; set; }

        [Parameter("Alpha", DefaultValue = 100, MinValue = 0, MaxValue = 255, Group = "Patterns Label")]
        public int PatternsLabelColorAlpha { get; set; }

        [Parameter("Locked", DefaultValue = true, Group = "Patterns Label")]
        public bool PatternsLabelLocked { get; set; }

        [Parameter("Link Style", DefaultValue = true, Group = "Patterns Label")]
        public bool PatternsLabelLinkStyle { get; set; }

        #endregion Patterns Label parameters

        #region Container Panel parameters

        [Parameter("Orientation", DefaultValue = Orientation.Vertical, Group = "Container Panel")]
        public Orientation PanelOrientation { get; set; }

        [Parameter("Horizontal Alignment", DefaultValue = HorizontalAlignment.Left, Group = "Container Panel")]
        public HorizontalAlignment PanelHorizontalAlignment { get; set; }

        [Parameter("Vertical Alignment", DefaultValue = VerticalAlignment.Top, Group = "Container Panel")]
        public VerticalAlignment PanelVerticalAlignment { get; set; }

        [Parameter("Margin", DefaultValue = 3, Group = "Container Panel")]
        public double PanelMargin { get; set; }

        #endregion Container Panel parameters

        #region Buttons parameters

        [Parameter("Disable Color", DefaultValue = "#FFCCCCCC", Group = "Buttons")]
        public string ButtonsBackgroundDisableColor { get; set; }

        [Parameter("Enable Color", DefaultValue = "Red", Group = "Buttons")]
        public string ButtonsBackgroundEnableColor { get; set; }

        [Parameter("Text Color", DefaultValue = "Blue", Group = "Buttons")]
        public string ButtonsForegroundColor { get; set; }

        [Parameter("Margin", DefaultValue = 1, Group = "Buttons")]
        public double ButtonsMargin { get; set; }

        [Parameter("Transparency", DefaultValue = 0.5, MinValue = 0, MaxValue = 1, Group = "Buttons")]
        public double ButtonsTransparency { get; set; }

        #endregion Buttons parameters

        #region TimeFrame Visibility parameters

        [Parameter("Enable", DefaultValue = false, Group = "TimeFrame Visibility")]
        public bool IsTimeFrameVisibilityEnabled { get; set; }

        [Parameter("TimeFrame", Group = "TimeFrame Visibility")]
        public TimeFrame VisibilityTimeFrame { get; set; }

        [Parameter("Only Buttons", Group = "TimeFrame Visibility")]
        public bool VisibilityOnlyButtons { get; set; }

        #endregion TimeFrame Visibility parameters

        #region Fibonacci Retracement parameters

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowFirstFibonacciRetracement { get; set; }

        [Parameter("Fill 1st Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillFirstFibonacciRetracement { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0, Group = "Fibonacci Retracement")]
        public double FirstFibonacciRetracementPercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Fibonacci Retracement")]
        public string FirstFibonacciRetracementColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int FirstFibonacciRetracementAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int FirstFibonacciRetracementThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle FirstFibonacciRetracementStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowSecondFibonacciRetracement { get; set; }

        [Parameter("Fill 2nd Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillSecondFibonacciRetracement { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.236, Group = "Fibonacci Retracement")]
        public double SecondFibonacciRetracementPercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Fibonacci Retracement")]
        public string SecondFibonacciRetracementColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int SecondFibonacciRetracementAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int SecondFibonacciRetracementThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle SecondFibonacciRetracementStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowThirdFibonacciRetracement { get; set; }

        [Parameter("Fill 3rd Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillThirdFibonacciRetracement { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.382, Group = "Fibonacci Retracement")]
        public double ThirdFibonacciRetracementPercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Fibonacci Retracement")]
        public string ThirdFibonacciRetracementColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int ThirdFibonacciRetracementAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int ThirdFibonacciRetracementThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle ThirdFibonacciRetracementStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowFourthFibonacciRetracement { get; set; }

        [Parameter("Fill 4th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillFourthFibonacciRetracement { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.5, Group = "Fibonacci Retracement")]
        public double FourthFibonacciRetracementPercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Fibonacci Retracement")]
        public string FourthFibonacciRetracementColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int FourthFibonacciRetracementAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int FourthFibonacciRetracementThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle FourthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowFifthFibonacciRetracement { get; set; }

        [Parameter("Fill 5th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillFifthFibonacciRetracement { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 0.618, Group = "Fibonacci Retracement")]
        public double FifthFibonacciRetracementPercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Fibonacci Retracement")]
        public string FifthFibonacciRetracementColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int FifthFibonacciRetracementAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int FifthFibonacciRetracementThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle FifthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowSixthFibonacciRetracement { get; set; }

        [Parameter("Fill 6th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillSixthFibonacciRetracement { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 0.786, Group = "Fibonacci Retracement")]
        public double SixthFibonacciRetracementPercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Fibonacci Retracement")]
        public string SixthFibonacciRetracementColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int SixthFibonacciRetracementAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int SixthFibonacciRetracementThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle SixthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowSeventhFibonacciRetracement { get; set; }

        [Parameter("Fill 7th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillSeventhFibonacciRetracement { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1, Group = "Fibonacci Retracement")]
        public double SeventhFibonacciRetracementPercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Fibonacci Retracement")]
        public string SeventhFibonacciRetracementColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int SeventhFibonacciRetracementAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int SeventhFibonacciRetracementThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle SeventhFibonacciRetracementStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowEighthFibonacciRetracement { get; set; }

        [Parameter("Fill 8th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillEighthFibonacciRetracement { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 1.618, Group = "Fibonacci Retracement")]
        public double EighthFibonacciRetracementPercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Fibonacci Retracement")]
        public string EighthFibonacciRetracementColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int EighthFibonacciRetracementAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int EighthFibonacciRetracementThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle EighthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowNinthFibonacciRetracement { get; set; }

        [Parameter("Fill 9th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillNinthFibonacciRetracement { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2.618, Group = "Fibonacci Retracement")]
        public double NinthFibonacciRetracementPercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Fibonacci Retracement")]
        public string NinthFibonacciRetracementColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int NinthFibonacciRetracementAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int NinthFibonacciRetracementThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle NinthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 10th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowTenthFibonacciRetracement { get; set; }

        [Parameter("Fill 10th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillTenthFibonacciRetracement { get; set; }

        [Parameter("10th Level Percent", DefaultValue = 3.618, Group = "Fibonacci Retracement")]
        public double TenthFibonacciRetracementPercent { get; set; }

        [Parameter("10th Level Color", DefaultValue = "Aquamarine", Group = "Fibonacci Retracement")]
        public string TenthFibonacciRetracementColor { get; set; }

        [Parameter("10th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int TenthFibonacciRetracementAlpha { get; set; }

        [Parameter("10th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int TenthFibonacciRetracementThickness { get; set; }

        [Parameter("10th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle TenthFibonacciRetracementStyle { get; set; }

        [Parameter("Show 11th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool ShowEleventhFibonacciRetracement { get; set; }

        [Parameter("Fill 11th Level", DefaultValue = true, Group = "Fibonacci Retracement")]
        public bool FillEleventhFibonacciRetracement { get; set; }

        [Parameter("11th Level Percent", DefaultValue = 4.236, Group = "Fibonacci Retracement")]
        public double EleventhFibonacciRetracementPercent { get; set; }

        [Parameter("11th Level Color", DefaultValue = "Chocolate", Group = "Fibonacci Retracement")]
        public string EleventhFibonacciRetracementColor { get; set; }

        [Parameter("11th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Retracement")]
        public int EleventhFibonacciRetracementAlpha { get; set; }

        [Parameter("11th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Retracement")]
        public int EleventhFibonacciRetracementThickness { get; set; }

        [Parameter("11th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Retracement")]
        public LineStyle EleventhFibonacciRetracementStyle { get; set; }

        #endregion Fibonacci Retracement parameters

        #region Fibonacci Speed Resistance Fan parameters

        [Parameter("Rectangle Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanRectangleThickness { get; set; }

        [Parameter("Rectangle Style", DefaultValue = LineStyle.Dots, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanRectangleStyle { get; set; }

        [Parameter("Rectangle Color", DefaultValue = "Blue", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanRectangleColor { get; set; }

        [Parameter("Extended Lines Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanExtendedLinesThickness { get; set; }

        [Parameter("Extended Lines Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanExtendedLinesStyle { get; set; }

        [Parameter("Extended Lines Color", DefaultValue = "Blue", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanExtendedLinesColor { get; set; }

        [Parameter("Price Levels Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanPriceLevelsThickness { get; set; }

        [Parameter("Price Levels Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanPriceLevelsStyle { get; set; }

        [Parameter("Price Levels Color", DefaultValue = "Magenta", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanPriceLevelsColor { get; set; }

        [Parameter("Time Levels Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanTimeLevelsThickness { get; set; }

        [Parameter("Time Levels Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanTimeLevelsStyle { get; set; }

        [Parameter("Time Levels Color", DefaultValue = "Yellow", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanTimeLevelsColor { get; set; }

        [Parameter("Main Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanMainFanThickness { get; set; }

        [Parameter("Main Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanMainFanStyle { get; set; }

        [Parameter("Main Fan Color", DefaultValue = "Yellow", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanMainFanColor { get; set; }

        [Parameter("1st Fan Percent", DefaultValue = 0.25, Group = "Fibonacci Speed Resistance Fan")]
        public double FibonacciSpeedResistanceFanFirstFanPercent { get; set; }

        [Parameter("1st Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanFirstFanThickness { get; set; }

        [Parameter("1st Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanFirstFanStyle { get; set; }

        [Parameter("1st Fan Color", DefaultValue = "Red", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanFirstFanColor { get; set; }

        [Parameter("2nd Fan Percent", DefaultValue = 0.382, Group = "Fibonacci Speed Resistance Fan")]
        public double FibonacciSpeedResistanceFanSecondFanPercent { get; set; }

        [Parameter("2nd Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanSecondFanThickness { get; set; }

        [Parameter("2nd Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanSecondFanStyle { get; set; }

        [Parameter("2nd Fan Color", DefaultValue = "Brown", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanSecondFanColor { get; set; }

        [Parameter("3rd Fan Percent", DefaultValue = 0.5, Group = "Fibonacci Speed Resistance Fan")]
        public double FibonacciSpeedResistanceFanThirdFanPercent { get; set; }

        [Parameter("3rd Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanThirdFanThickness { get; set; }

        [Parameter("3rd Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanThirdFanStyle { get; set; }

        [Parameter("3rd Fan Color", DefaultValue = "Lime", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanThirdFanColor { get; set; }

        [Parameter("4th Fan Percent", DefaultValue = 0.618, Group = "Fibonacci Speed Resistance Fan")]
        public double FibonacciSpeedResistanceFanFourthFanPercent { get; set; }

        [Parameter("4th Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanFourthFanThickness { get; set; }

        [Parameter("4th Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanFourthFanStyle { get; set; }

        [Parameter("4th Fan Color", DefaultValue = "Magenta", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanFourthFanColor { get; set; }

        [Parameter("5th Fan Percent", DefaultValue = 0.75, Group = "Fibonacci Speed Resistance Fan")]
        public double FibonacciSpeedResistanceFanFifthFanPercent { get; set; }

        [Parameter("5th Fan Thickness", DefaultValue = 1, MinValue = 1, Group = "Fibonacci Speed Resistance Fan")]
        public int FibonacciSpeedResistanceFanFifthFanThickness { get; set; }

        [Parameter("5th Fan Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Speed Resistance Fan")]
        public LineStyle FibonacciSpeedResistanceFanFifthFanStyle { get; set; }

        [Parameter("5th Fan Color", DefaultValue = "Blue", Group = "Fibonacci Speed Resistance Fan")]
        public string FibonacciSpeedResistanceFanFifthFanColor { get; set; }

        #endregion Fibonacci Speed Resistance Fan parameters

        #region Fibonacci Time Zone parameters

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowFirstFibonacciTimeZone { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0, Group = "Fibonacci Time Zone")]
        public double FirstFibonacciTimeZonePercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Fibonacci Time Zone")]
        public string FirstFibonacciTimeZoneColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int FirstFibonacciTimeZoneAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int FirstFibonacciTimeZoneThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle FirstFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowSecondFibonacciTimeZone { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 1, Group = "Fibonacci Time Zone")]
        public double SecondFibonacciTimeZonePercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Fibonacci Time Zone")]
        public string SecondFibonacciTimeZoneColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int SecondFibonacciTimeZoneAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int SecondFibonacciTimeZoneThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle SecondFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowThirdFibonacciTimeZone { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 2, Group = "Fibonacci Time Zone")]
        public double ThirdFibonacciTimeZonePercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Fibonacci Time Zone")]
        public string ThirdFibonacciTimeZoneColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int ThirdFibonacciTimeZoneAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int ThirdFibonacciTimeZoneThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle ThirdFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowFourthFibonacciTimeZone { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 3, Group = "Fibonacci Time Zone")]
        public double FourthFibonacciTimeZonePercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Fibonacci Time Zone")]
        public string FourthFibonacciTimeZoneColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int FourthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int FourthFibonacciTimeZoneThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle FourthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowFifthFibonacciTimeZone { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 5, Group = "Fibonacci Time Zone")]
        public double FifthFibonacciTimeZonePercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Fibonacci Time Zone")]
        public string FifthFibonacciTimeZoneColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int FifthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int FifthFibonacciTimeZoneThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle FifthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowSixthFibonacciTimeZone { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 8, Group = "Fibonacci Time Zone")]
        public double SixthFibonacciTimeZonePercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Fibonacci Time Zone")]
        public string SixthFibonacciTimeZoneColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int SixthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int SixthFibonacciTimeZoneThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle SixthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowSeventhFibonacciTimeZone { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 13, Group = "Fibonacci Time Zone")]
        public double SeventhFibonacciTimeZonePercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Fibonacci Time Zone")]
        public string SeventhFibonacciTimeZoneColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int SeventhFibonacciTimeZoneAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int SeventhFibonacciTimeZoneThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle SeventhFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowEighthFibonacciTimeZone { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 21, Group = "Fibonacci Time Zone")]
        public double EighthFibonacciTimeZonePercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Fibonacci Time Zone")]
        public string EighthFibonacciTimeZoneColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int EighthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int EighthFibonacciTimeZoneThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle EighthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowNinthFibonacciTimeZone { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 34, Group = "Fibonacci Time Zone")]
        public double NinthFibonacciTimeZonePercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Fibonacci Time Zone")]
        public string NinthFibonacciTimeZoneColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int NinthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int NinthFibonacciTimeZoneThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle NinthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 10th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowTenthFibonacciTimeZone { get; set; }

        [Parameter("10th Level Percent", DefaultValue = 55, Group = "Fibonacci Time Zone")]
        public double TenthFibonacciTimeZonePercent { get; set; }

        [Parameter("10th Level Color", DefaultValue = "Aquamarine", Group = "Fibonacci Time Zone")]
        public string TenthFibonacciTimeZoneColor { get; set; }

        [Parameter("10th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int TenthFibonacciTimeZoneAlpha { get; set; }

        [Parameter("10th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int TenthFibonacciTimeZoneThickness { get; set; }

        [Parameter("10th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle TenthFibonacciTimeZoneStyle { get; set; }

        [Parameter("Show 11th Level", DefaultValue = true, Group = "Fibonacci Time Zone")]
        public bool ShowEleventhFibonacciTimeZone { get; set; }

        [Parameter("11th Level Percent", DefaultValue = 89, Group = "Fibonacci Time Zone")]
        public double EleventhFibonacciTimeZonePercent { get; set; }

        [Parameter("11th Level Color", DefaultValue = "Chocolate", Group = "Fibonacci Time Zone")]
        public string EleventhFibonacciTimeZoneColor { get; set; }

        [Parameter("11th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Fibonacci Time Zone")]
        public int EleventhFibonacciTimeZoneAlpha { get; set; }

        [Parameter("11th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Time Zone")]
        public int EleventhFibonacciTimeZoneThickness { get; set; }

        [Parameter("11th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Time Zone")]
        public LineStyle EleventhFibonacciTimeZoneStyle { get; set; }

        #endregion Fibonacci Time Zone parameters

        #region Trend Based Fibonacci Time Parameters

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowFirstTrendBasedFibonacciTime { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0, Group = "Trend Based Fibonacci Time")]
        public double FirstTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Trend Based Fibonacci Time")]
        public string FirstTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int FirstTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int FirstTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle FirstTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowSecondTrendBasedFibonacciTime { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.382, Group = "Trend Based Fibonacci Time")]
        public double SecondTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Trend Based Fibonacci Time")]
        public string SecondTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int SecondTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int SecondTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle SecondTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowThirdTrendBasedFibonacciTime { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.5, Group = "Trend Based Fibonacci Time")]
        public double ThirdTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Trend Based Fibonacci Time")]
        public string ThirdTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int ThirdTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int ThirdTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle ThirdTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowFourthTrendBasedFibonacciTime { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.618, Group = "Trend Based Fibonacci Time")]
        public double FourthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Trend Based Fibonacci Time")]
        public string FourthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int FourthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int FourthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle FourthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowFifthTrendBasedFibonacciTime { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 1, Group = "Trend Based Fibonacci Time")]
        public double FifthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Trend Based Fibonacci Time")]
        public string FifthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int FifthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int FifthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle FifthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowSixthTrendBasedFibonacciTime { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 1.382, Group = "Trend Based Fibonacci Time")]
        public double SixthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Trend Based Fibonacci Time")]
        public string SixthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int SixthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int SixthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle SixthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowSeventhTrendBasedFibonacciTime { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1.618, Group = "Trend Based Fibonacci Time")]
        public double SeventhTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Trend Based Fibonacci Time")]
        public string SeventhTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int SeventhTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int SeventhTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle SeventhTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowEighthTrendBasedFibonacciTime { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 2, Group = "Trend Based Fibonacci Time")]
        public double EighthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Trend Based Fibonacci Time")]
        public string EighthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int EighthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int EighthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle EighthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowNinthTrendBasedFibonacciTime { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2.382, Group = "Trend Based Fibonacci Time")]
        public double NinthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Trend Based Fibonacci Time")]
        public string NinthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int NinthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int NinthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle NinthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 10th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowTenthTrendBasedFibonacciTime { get; set; }

        [Parameter("10th Level Percent", DefaultValue = 2.618, Group = "Trend Based Fibonacci Time")]
        public double TenthTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("10th Level Color", DefaultValue = "Aquamarine", Group = "Trend Based Fibonacci Time")]
        public string TenthTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("10th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int TenthTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("10th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int TenthTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("10th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle TenthTrendBasedFibonacciTimeStyle { get; set; }

        [Parameter("Show 11th Level", DefaultValue = true, Group = "Trend Based Fibonacci Time")]
        public bool ShowEleventhTrendBasedFibonacciTime { get; set; }

        [Parameter("11th Level Percent", DefaultValue = 3, Group = "Trend Based Fibonacci Time")]
        public double EleventhTrendBasedFibonacciTimePercent { get; set; }

        [Parameter("11th Level Color", DefaultValue = "Chocolate", Group = "Trend Based Fibonacci Time")]
        public string EleventhTrendBasedFibonacciTimeColor { get; set; }

        [Parameter("11th Level Alpha", DefaultValue = 150, MinValue = 0, MaxValue = 255, Group = "Trend Based Fibonacci Time")]
        public int EleventhTrendBasedFibonacciTimeAlpha { get; set; }

        [Parameter("11th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Trend Based Fibonacci Time")]
        public int EleventhTrendBasedFibonacciTimeThickness { get; set; }

        [Parameter("11th Level Style", DefaultValue = LineStyle.Solid, Group = "Trend Based Fibonacci Time")]
        public LineStyle EleventhTrendBasedFibonacciTimeStyle { get; set; }

        #endregion Trend Based Fibonacci Time Parameters

        #region Fibonacci Channel parameters

        [Parameter("Show 1st Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowFirstFibonacciChannel { get; set; }

        [Parameter("Fill 1st Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillFirstFibonacciChannel { get; set; }

        [Parameter("1st Level Percent", DefaultValue = 0, Group = "Fibonacci Channel")]
        public double FirstFibonacciChannelPercent { get; set; }

        [Parameter("1st Level Color", DefaultValue = "Gray", Group = "Fibonacci Channel")]
        public string FirstFibonacciChannelColor { get; set; }

        [Parameter("1st Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int FirstFibonacciChannelAlpha { get; set; }

        [Parameter("1st Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int FirstFibonacciChannelThickness { get; set; }

        [Parameter("1st Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle FirstFibonacciChannelStyle { get; set; }

        [Parameter("Show 2nd Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowSecondFibonacciChannel { get; set; }

        [Parameter("Fill 2nd Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillSecondFibonacciChannel { get; set; }

        [Parameter("2nd Level Percent", DefaultValue = 0.236, Group = "Fibonacci Channel")]
        public double SecondFibonacciChannelPercent { get; set; }

        [Parameter("2nd Level Color", DefaultValue = "Red", Group = "Fibonacci Channel")]
        public string SecondFibonacciChannelColor { get; set; }

        [Parameter("2nd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int SecondFibonacciChannelAlpha { get; set; }

        [Parameter("2nd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int SecondFibonacciChannelThickness { get; set; }

        [Parameter("2nd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle SecondFibonacciChannelStyle { get; set; }

        [Parameter("Show 3rd Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowThirdFibonacciChannel { get; set; }

        [Parameter("Fill 3rd Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillThirdFibonacciChannel { get; set; }

        [Parameter("3rd Level Percent", DefaultValue = 0.382, Group = "Fibonacci Channel")]
        public double ThirdFibonacciChannelPercent { get; set; }

        [Parameter("3rd Level Color", DefaultValue = "GreenYellow", Group = "Fibonacci Channel")]
        public string ThirdFibonacciChannelColor { get; set; }

        [Parameter("3rd Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int ThirdFibonacciChannelAlpha { get; set; }

        [Parameter("3rd Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int ThirdFibonacciChannelThickness { get; set; }

        [Parameter("3rd Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle ThirdFibonacciChannelStyle { get; set; }

        [Parameter("Show 4th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowFourthFibonacciChannel { get; set; }

        [Parameter("Fill 4th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillFourthFibonacciChannel { get; set; }

        [Parameter("4th Level Percent", DefaultValue = 0.5, Group = "Fibonacci Channel")]
        public double FourthFibonacciChannelPercent { get; set; }

        [Parameter("4th Level Color", DefaultValue = "DarkGreen", Group = "Fibonacci Channel")]
        public string FourthFibonacciChannelColor { get; set; }

        [Parameter("4th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int FourthFibonacciChannelAlpha { get; set; }

        [Parameter("4th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int FourthFibonacciChannelThickness { get; set; }

        [Parameter("4th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle FourthFibonacciChannelStyle { get; set; }

        [Parameter("Show 5th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowFifthFibonacciChannel { get; set; }

        [Parameter("Fill 5th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillFifthFibonacciChannel { get; set; }

        [Parameter("5th Level Percent", DefaultValue = 0.618, Group = "Fibonacci Channel")]
        public double FifthFibonacciChannelPercent { get; set; }

        [Parameter("5th Level Color", DefaultValue = "BlueViolet", Group = "Fibonacci Channel")]
        public string FifthFibonacciChannelColor { get; set; }

        [Parameter("5th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int FifthFibonacciChannelAlpha { get; set; }

        [Parameter("5th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int FifthFibonacciChannelThickness { get; set; }

        [Parameter("5th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle FifthFibonacciChannelStyle { get; set; }

        [Parameter("Show 6th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowSixthFibonacciChannel { get; set; }

        [Parameter("Fill 6th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillSixthFibonacciChannel { get; set; }

        [Parameter("6th Level Percent", DefaultValue = 0.786, Group = "Fibonacci Channel")]
        public double SixthFibonacciChannelPercent { get; set; }

        [Parameter("6th Level Color", DefaultValue = "AliceBlue", Group = "Fibonacci Channel")]
        public string SixthFibonacciChannelColor { get; set; }

        [Parameter("6th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int SixthFibonacciChannelAlpha { get; set; }

        [Parameter("6th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int SixthFibonacciChannelThickness { get; set; }

        [Parameter("6th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle SixthFibonacciChannelStyle { get; set; }

        [Parameter("Show 7th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowSeventhFibonacciChannel { get; set; }

        [Parameter("Fill 7th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillSeventhFibonacciChannel { get; set; }

        [Parameter("7th Level Percent", DefaultValue = 1, Group = "Fibonacci Channel")]
        public double SeventhFibonacciChannelPercent { get; set; }

        [Parameter("7th Level Color", DefaultValue = "Bisque", Group = "Fibonacci Channel")]
        public string SeventhFibonacciChannelColor { get; set; }

        [Parameter("7th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int SeventhFibonacciChannelAlpha { get; set; }

        [Parameter("7th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int SeventhFibonacciChannelThickness { get; set; }

        [Parameter("7th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle SeventhFibonacciChannelStyle { get; set; }

        [Parameter("Show 8th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowEighthFibonacciChannel { get; set; }

        [Parameter("Fill 8th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillEighthFibonacciChannel { get; set; }

        [Parameter("8th Level Percent", DefaultValue = 1.618, Group = "Fibonacci Channel")]
        public double EighthFibonacciChannelPercent { get; set; }

        [Parameter("8th Level Color", DefaultValue = "Azure", Group = "Fibonacci Channel")]
        public string EighthFibonacciChannelColor { get; set; }

        [Parameter("8th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int EighthFibonacciChannelAlpha { get; set; }

        [Parameter("8th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int EighthFibonacciChannelThickness { get; set; }

        [Parameter("8th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle EighthFibonacciChannelStyle { get; set; }

        [Parameter("Show 9th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowNinthFibonacciChannel { get; set; }

        [Parameter("Fill 9th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillNinthFibonacciChannel { get; set; }

        [Parameter("9th Level Percent", DefaultValue = 2.618, Group = "Fibonacci Channel")]
        public double NinthFibonacciChannelPercent { get; set; }

        [Parameter("9th Level Color", DefaultValue = "Aqua", Group = "Fibonacci Channel")]
        public string NinthFibonacciChannelColor { get; set; }

        [Parameter("9th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int NinthFibonacciChannelAlpha { get; set; }

        [Parameter("9th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int NinthFibonacciChannelThickness { get; set; }

        [Parameter("9th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle NinthFibonacciChannelStyle { get; set; }

        [Parameter("Show 10th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowTenthFibonacciChannel { get; set; }

        [Parameter("Fill 10th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillTenthFibonacciChannel { get; set; }

        [Parameter("10th Level Percent", DefaultValue = 3.618, Group = "Fibonacci Channel")]
        public double TenthFibonacciChannelPercent { get; set; }

        [Parameter("10th Level Color", DefaultValue = "Aquamarine", Group = "Fibonacci Channel")]
        public string TenthFibonacciChannelColor { get; set; }

        [Parameter("10th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int TenthFibonacciChannelAlpha { get; set; }

        [Parameter("10th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int TenthFibonacciChannelThickness { get; set; }

        [Parameter("10th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle TenthFibonacciChannelStyle { get; set; }

        [Parameter("Show 11th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool ShowEleventhFibonacciChannel { get; set; }

        [Parameter("Fill 11th Level", DefaultValue = true, Group = "Fibonacci Channel")]
        public bool FillEleventhFibonacciChannel { get; set; }

        [Parameter("11th Level Percent", DefaultValue = 4.236, Group = "Fibonacci Channel")]
        public double EleventhFibonacciChannelPercent { get; set; }

        [Parameter("11th Level Color", DefaultValue = "Chocolate", Group = "Fibonacci Channel")]
        public string EleventhFibonacciChannelColor { get; set; }

        [Parameter("11th Level Alpha", DefaultValue = 50, MinValue = 0, MaxValue = 255, Group = "Fibonacci Channel")]
        public int EleventhFibonacciChannelAlpha { get; set; }

        [Parameter("11th Level Thickness", DefaultValue = 1, MinValue = 0, Group = "Fibonacci Channel")]
        public int EleventhFibonacciChannelThickness { get; set; }

        [Parameter("11th Level Style", DefaultValue = LineStyle.Solid, Group = "Fibonacci Channel")]
        public LineStyle EleventhFibonacciChannelStyle { get; set; }

        #endregion Fibonacci Channel parameters

        #region Overridden methods

        protected override void Initialize()
        {
            _mainPanel = new StackPanel
            {
                HorizontalAlignment = PanelHorizontalAlignment,
                VerticalAlignment = PanelVerticalAlignment,
                Orientation = PanelOrientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal,
                BackgroundColor = Color.Transparent,
            };

            _mainButtonsPanel = new StackPanel
            {
                Orientation = PanelOrientation,
                Margin = PanelMargin
            };

            _mainPanel.AddChild(_mainButtonsPanel);

            _groupButtonsPanel = new StackPanel
            {
                Orientation = PanelOrientation,
                Margin = PanelMargin,
                IsVisible = false
            };

            _mainPanel.AddChild(_groupButtonsPanel);

            _buttonsBackgroundDisableColor = ColorParser.Parse(ButtonsBackgroundDisableColor);
            _buttonsBackgroundEnableColor = ColorParser.Parse(ButtonsBackgroundEnableColor);

            _buttonsStyle = new Style();

            _buttonsStyle.Set(ControlProperty.Margin, ButtonsMargin);
            _buttonsStyle.Set(ControlProperty.BackgroundColor, _buttonsBackgroundDisableColor);
            _buttonsStyle.Set(ControlProperty.ForegroundColor, ColorParser.Parse(ButtonsForegroundColor));
            _buttonsStyle.Set(ControlProperty.HorizontalContentAlignment, HorizontalAlignment.Center);
            _buttonsStyle.Set(ControlProperty.VerticalContentAlignment, VerticalAlignment.Center);
            _buttonsStyle.Set(ControlProperty.Opacity, ButtonsTransparency);

            var patternsColor = ColorParser.Parse(PatternsColor, PatternsColorAlpha);
            var patternsLabelsColor = ColorParser.Parse(PatternsLabelColor, PatternsLabelColorAlpha);

            var patternConfig = new PatternConfig(Chart, patternsColor, PatternsLabelShow, patternsLabelsColor, PatternsLabelLocked, PatternsLabelLinkStyle, new Logger(this.GetType().Name, Print));

            _expandButton = new Button
            {
                Style = _buttonsStyle,
                Text = "Expand Patterns"
            };

            _expandButton.Click += ExpandButton_Click;

            _mainButtonsPanel.AddChild(_expandButton);

            AddPatternButton(new FibonacciRetracementPattern(patternConfig, GetFibonacciRetracementLevels()));
            AddPatternButton(new FibonacciSpeedResistanceFanPattern(patternConfig, new FibonacciSpeedResistanceFanSettings
            {
                RectangleThickness = FibonacciSpeedResistanceFanRectangleThickness,
                RectangleStyle = FibonacciSpeedResistanceFanRectangleStyle,
                RectangleColor = ColorParser.Parse(FibonacciSpeedResistanceFanRectangleColor),
                PriceLevelsThickness = FibonacciSpeedResistanceFanPriceLevelsThickness,
                PriceLevelsStyle = FibonacciSpeedResistanceFanPriceLevelsStyle,
                PriceLevelsColor = ColorParser.Parse(FibonacciSpeedResistanceFanPriceLevelsColor),
                TimeLevelsThickness = FibonacciSpeedResistanceFanTimeLevelsThickness,
                TimeLevelsStyle = FibonacciSpeedResistanceFanTimeLevelsStyle,
                TimeLevelsColor = ColorParser.Parse(FibonacciSpeedResistanceFanTimeLevelsColor),
                ExtendedLinesThickness = FibonacciSpeedResistanceFanExtendedLinesThickness,
                ExtendedLinesStyle = FibonacciSpeedResistanceFanExtendedLinesStyle,
                ExtendedLinesColor = ColorParser.Parse(FibonacciSpeedResistanceFanExtendedLinesColor),
                MainFanSettings = new FanSettings
                {
                    Color = ColorParser.Parse(FibonacciSpeedResistanceFanMainFanColor),
                    Style = FibonacciSpeedResistanceFanMainFanStyle,
                    Thickness = FibonacciSpeedResistanceFanMainFanThickness
                },
                SideFanSettings = new SideFanSettings[]
                {
                        new SideFanSettings
                        {
                            Name = "1x2",
                            Percent = FibonacciSpeedResistanceFanFirstFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFirstFanColor),
                            Style = FibonacciSpeedResistanceFanFirstFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFirstFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "1x3",
                            Percent = FibonacciSpeedResistanceFanSecondFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanSecondFanColor),
                            Style = FibonacciSpeedResistanceFanSecondFanStyle,
                            Thickness = FibonacciSpeedResistanceFanSecondFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "1x4",
                            Percent = FibonacciSpeedResistanceFanThirdFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanThirdFanColor),
                            Style = FibonacciSpeedResistanceFanThirdFanStyle,
                            Thickness = FibonacciSpeedResistanceFanThirdFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "1x8",
                            Percent = FibonacciSpeedResistanceFanFourthFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFourthFanColor),
                            Style = FibonacciSpeedResistanceFanFourthFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFourthFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "1x9",
                            Percent = FibonacciSpeedResistanceFanFifthFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFifthFanColor),
                            Style = FibonacciSpeedResistanceFanFifthFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFifthFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "2x1",
                            Percent = -FibonacciSpeedResistanceFanFirstFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFirstFanColor),
                            Style = FibonacciSpeedResistanceFanFirstFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFirstFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "3x1",
                            Percent = -FibonacciSpeedResistanceFanSecondFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanSecondFanColor),
                            Style = FibonacciSpeedResistanceFanSecondFanStyle,
                            Thickness = FibonacciSpeedResistanceFanSecondFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "4x1",
                            Percent = -FibonacciSpeedResistanceFanThirdFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanThirdFanColor),
                            Style = FibonacciSpeedResistanceFanThirdFanStyle,
                            Thickness = FibonacciSpeedResistanceFanThirdFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "8x1",
                            Percent = -FibonacciSpeedResistanceFanFourthFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFourthFanColor),
                            Style = FibonacciSpeedResistanceFanFourthFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFourthFanThickness
                        },
                        new SideFanSettings
                        {
                            Name = "9x1",
                            Percent = -FibonacciSpeedResistanceFanFifthFanPercent,
                            Color = ColorParser.Parse(FibonacciSpeedResistanceFanFifthFanColor),
                            Style = FibonacciSpeedResistanceFanFifthFanStyle,
                            Thickness = FibonacciSpeedResistanceFanFifthFanThickness
                        }
                }
            }));
            AddPatternButton(new FibonacciTimeZonePattern(patternConfig, GetFibonacciTimeZoneLevels()));
            AddPatternButton(new TrendBasedFibonacciTimePattern(patternConfig, GetTrendBasedFibonacciTimeLevels()));
            AddPatternButton(new FibonacciChannelPattern(patternConfig, GetFibonacciChannelLevels()));

            var showHideButton = new Controls.ToggleButton()
            {
                Style = _buttonsStyle,
                OnColor = _buttonsBackgroundEnableColor,
                OffColor = _buttonsBackgroundDisableColor,
                Text = "Hide",
                IsVisible = false
            };

            showHideButton.TurnedOn += ShowHideButton_TurnedOn;
            showHideButton.TurnedOff += ShowHideButton_TurnedOff;

            _mainButtonsPanel.AddChild(showHideButton);
            _buttons.Add(showHideButton);

            var saveButton = new PatternsSaveButton(Chart)
            {
                Style = _buttonsStyle,
                IsVisible = false
            };

            _mainButtonsPanel.AddChild(saveButton);
            _buttons.Add(saveButton);

            var loadButton = new PatternsLoadButton(Chart)
            {
                Style = _buttonsStyle,
                IsVisible = false
            };

            _mainButtonsPanel.AddChild(loadButton);
            _buttons.Add(loadButton);

            var removeAllButton = new PatternsRemoveAllButton(Chart)
            {
                Style = _buttonsStyle,
                IsVisible = false
            };

            _mainButtonsPanel.AddChild(removeAllButton);
            _buttons.Add(removeAllButton);

            var collapseButton = new Button
            {
                Style = _buttonsStyle,
                Text = "Collapse",
                IsVisible = false
            };

            collapseButton.Click += CollapseButton_Click;

            _mainButtonsPanel.AddChild(collapseButton);
            _buttons.Add(collapseButton);

            Chart.AddControl(_mainPanel);

            CheckTimeFrameVisibility();
        }

        public override void Calculate(int index)
        {
        }

        #endregion Overridden methods

        private void CollapseButton_Click(ButtonClickEventArgs obj)
        {
            _buttons.ForEach(iButton => iButton.IsVisible = false);

            _groupButtonsPanel.IsVisible = false;

            _expandButton.IsVisible = true;
        }

        private void ExpandButton_Click(ButtonClickEventArgs obj)
        {
            _buttons.ForEach(iButton => iButton.IsVisible = true);

            obj.Button.IsVisible = false;
        }

        private void ShowHideButton_TurnedOff(Controls.ToggleButton obj)
        {
            Chart.ChangePatternsVisibility(false);

            obj.Text = "Hide";
        }

        private void ShowHideButton_TurnedOn(Controls.ToggleButton obj)
        {
            Chart.ChangePatternsVisibility(true);

            obj.Text = "Show";
        }

        private void AddPatternButton(IPattern pattern)
        {
            var button = new PatternButton(pattern)
            {
                Style = _buttonsStyle,
                OnColor = _buttonsBackgroundEnableColor,
                OffColor = _buttonsBackgroundDisableColor,
                IsVisible = false
            };

            _buttons.Add(button);

            _mainButtonsPanel.AddChild(button);

            pattern.Initialize();
        }

        private PatternGroupButton AddPatternGroupButton(string text)
        {
            var groupButton = new PatternGroupButton(_groupButtonsPanel)
            {
                Text = text,
                Style = _buttonsStyle,
                OnColor = _buttonsBackgroundEnableColor,
                OffColor = _buttonsBackgroundDisableColor,
                IsVisible = false
            };

            _buttons.Add(groupButton);

            _mainButtonsPanel.AddChild(groupButton);

            return groupButton;
        }

        private void CheckTimeFrameVisibility()
        {
            if (IsTimeFrameVisibilityEnabled)
            {
                if (TimeFrame != VisibilityTimeFrame)
                {
                    _mainButtonsPanel.IsVisible = false;

                    if (!VisibilityOnlyButtons) Chart.ChangePatternsVisibility(true);
                }
                else if (!VisibilityOnlyButtons)
                {
                    Chart.ChangePatternsVisibility(false);
                }
            }
        }

        private IEnumerable<Patterns.FibonacciLevel> GetFibonacciRetracementLevels()
        {
            var fibonacciRetracementLevels = new List<Patterns.FibonacciLevel>();

            if (ShowFirstFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = FirstFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(FirstFibonacciRetracementColor),
                    Style = FirstFibonacciRetracementStyle,
                    Thickness = FirstFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(FirstFibonacciRetracementColor, FirstFibonacciRetracementAlpha),
                    IsFilled = FillFirstFibonacciRetracement,
                });
            }

            if (ShowSecondFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = SecondFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(SecondFibonacciRetracementColor),
                    Style = SecondFibonacciRetracementStyle,
                    Thickness = SecondFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(SecondFibonacciRetracementColor, SecondFibonacciRetracementAlpha),
                    IsFilled = FillSecondFibonacciRetracement,
                });
            }

            if (ShowThirdFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = ThirdFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(ThirdFibonacciRetracementColor),
                    Style = ThirdFibonacciRetracementStyle,
                    Thickness = ThirdFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(ThirdFibonacciRetracementColor, ThirdFibonacciRetracementAlpha),
                    IsFilled = FillThirdFibonacciRetracement,
                });
            }

            if (ShowFourthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = FourthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(FourthFibonacciRetracementColor),
                    Style = FourthFibonacciRetracementStyle,
                    Thickness = FourthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(FourthFibonacciRetracementColor, FourthFibonacciRetracementAlpha),
                    IsFilled = FillFourthFibonacciRetracement,
                });
            }

            if (ShowFifthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = FifthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(FifthFibonacciRetracementColor),
                    Style = FifthFibonacciRetracementStyle,
                    Thickness = FifthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(FifthFibonacciRetracementColor, FifthFibonacciRetracementAlpha),
                    IsFilled = FillFifthFibonacciRetracement,
                });
            }

            if (ShowSixthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = SixthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(SixthFibonacciRetracementColor),
                    Style = SixthFibonacciRetracementStyle,
                    Thickness = SixthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(SixthFibonacciRetracementColor, SixthFibonacciRetracementAlpha),
                    IsFilled = FillSixthFibonacciRetracement,
                });
            }

            if (ShowSeventhFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = SeventhFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(SeventhFibonacciRetracementColor),
                    Style = SeventhFibonacciRetracementStyle,
                    Thickness = SeventhFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(SeventhFibonacciRetracementColor, SeventhFibonacciRetracementAlpha),
                    IsFilled = FillSeventhFibonacciRetracement,
                });
            }

            if (ShowEighthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = EighthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(EighthFibonacciRetracementColor),
                    Style = EighthFibonacciRetracementStyle,
                    Thickness = EighthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(EighthFibonacciRetracementColor, EighthFibonacciRetracementAlpha),
                    IsFilled = FillEighthFibonacciRetracement,
                });
            }

            if (ShowNinthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = NinthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(NinthFibonacciRetracementColor),
                    Style = NinthFibonacciRetracementStyle,
                    Thickness = NinthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(NinthFibonacciRetracementColor, NinthFibonacciRetracementAlpha),
                    IsFilled = FillNinthFibonacciRetracement,
                });
            }

            if (ShowTenthFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = TenthFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(TenthFibonacciRetracementColor),
                    Style = TenthFibonacciRetracementStyle,
                    Thickness = TenthFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(TenthFibonacciRetracementColor, TenthFibonacciRetracementAlpha),
                    IsFilled = FillTenthFibonacciRetracement,
                });
            }

            if (ShowEleventhFibonacciRetracement)
            {
                fibonacciRetracementLevels.Add(new Patterns.FibonacciLevel
                {
                    Percent = EleventhFibonacciRetracementPercent,
                    LineColor = ColorParser.Parse(EleventhFibonacciRetracementColor),
                    Style = EleventhFibonacciRetracementStyle,
                    Thickness = EleventhFibonacciRetracementThickness,
                    FillColor = ColorParser.Parse(EleventhFibonacciRetracementColor, EleventhFibonacciRetracementAlpha),
                    IsFilled = FillEleventhFibonacciRetracement,
                });
            }

            return fibonacciRetracementLevels;
        }

        private IEnumerable<Patterns.FibonacciLevel> GetFibonacciTimeZoneLevels()
        {
            var result = new List<Patterns.FibonacciLevel>();

            if (ShowFirstFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FirstFibonacciTimeZonePercent,
                    Style = FirstFibonacciTimeZoneStyle,
                    Thickness = FirstFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(FirstFibonacciTimeZoneColor, FirstFibonacciTimeZoneAlpha),
                });
            }

            if (ShowSecondFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SecondFibonacciTimeZonePercent,
                    Style = SecondFibonacciTimeZoneStyle,
                    Thickness = SecondFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(SecondFibonacciTimeZoneColor, SecondFibonacciTimeZoneAlpha),
                });
            }

            if (ShowThirdFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = ThirdFibonacciTimeZonePercent,
                    Style = ThirdFibonacciTimeZoneStyle,
                    Thickness = ThirdFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(ThirdFibonacciTimeZoneColor, ThirdFibonacciTimeZoneAlpha),
                });
            }

            if (ShowFourthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FourthFibonacciTimeZonePercent,
                    Style = FourthFibonacciTimeZoneStyle,
                    Thickness = FourthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(FourthFibonacciTimeZoneColor, FourthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowFifthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FifthFibonacciTimeZonePercent,
                    Style = FifthFibonacciTimeZoneStyle,
                    Thickness = FifthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(FifthFibonacciTimeZoneColor, FifthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowSixthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SixthFibonacciTimeZonePercent,
                    Style = SixthFibonacciTimeZoneStyle,
                    Thickness = SixthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(SixthFibonacciTimeZoneColor, SixthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowSeventhFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SeventhFibonacciTimeZonePercent,
                    Style = SeventhFibonacciTimeZoneStyle,
                    Thickness = SeventhFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(SeventhFibonacciTimeZoneColor, SeventhFibonacciTimeZoneAlpha),
                });
            }

            if (ShowEighthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EighthFibonacciTimeZonePercent,
                    Style = EighthFibonacciTimeZoneStyle,
                    Thickness = EighthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(EighthFibonacciTimeZoneColor, EighthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowNinthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = NinthFibonacciTimeZonePercent,
                    Style = NinthFibonacciTimeZoneStyle,
                    Thickness = NinthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(NinthFibonacciTimeZoneColor, NinthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowTenthFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = TenthFibonacciTimeZonePercent,
                    Style = TenthFibonacciTimeZoneStyle,
                    Thickness = TenthFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(TenthFibonacciTimeZoneColor, TenthFibonacciTimeZoneAlpha),
                });
            }

            if (ShowEleventhFibonacciTimeZone)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EleventhFibonacciTimeZonePercent,
                    Style = EleventhFibonacciTimeZoneStyle,
                    Thickness = EleventhFibonacciTimeZoneThickness,
                    LineColor = ColorParser.Parse(EleventhFibonacciTimeZoneColor, EleventhFibonacciTimeZoneAlpha),
                });
            }

            return result;
        }

        private IEnumerable<Patterns.FibonacciLevel> GetTrendBasedFibonacciTimeLevels()
        {
            var result = new List<Patterns.FibonacciLevel>();

            if (ShowFirstTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FirstTrendBasedFibonacciTimePercent,
                    Style = FirstTrendBasedFibonacciTimeStyle,
                    Thickness = FirstTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(FirstTrendBasedFibonacciTimeColor, FirstTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowSecondTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SecondTrendBasedFibonacciTimePercent,
                    Style = SecondTrendBasedFibonacciTimeStyle,
                    Thickness = SecondTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(SecondTrendBasedFibonacciTimeColor, SecondTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowThirdTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = ThirdTrendBasedFibonacciTimePercent,
                    Style = ThirdTrendBasedFibonacciTimeStyle,
                    Thickness = ThirdTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(ThirdTrendBasedFibonacciTimeColor, ThirdTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowFourthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FourthTrendBasedFibonacciTimePercent,
                    Style = FourthTrendBasedFibonacciTimeStyle,
                    Thickness = FourthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(FourthTrendBasedFibonacciTimeColor, FourthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowFifthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FifthTrendBasedFibonacciTimePercent,
                    Style = FifthTrendBasedFibonacciTimeStyle,
                    Thickness = FifthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(FifthTrendBasedFibonacciTimeColor, FifthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowSixthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SixthTrendBasedFibonacciTimePercent,
                    Style = SixthTrendBasedFibonacciTimeStyle,
                    Thickness = SixthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(SixthTrendBasedFibonacciTimeColor, SixthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowSeventhTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SeventhTrendBasedFibonacciTimePercent,
                    Style = SeventhTrendBasedFibonacciTimeStyle,
                    Thickness = SeventhTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(SeventhTrendBasedFibonacciTimeColor, SeventhTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowEighthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EighthTrendBasedFibonacciTimePercent,
                    Style = EighthTrendBasedFibonacciTimeStyle,
                    Thickness = EighthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(EighthTrendBasedFibonacciTimeColor, EighthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowNinthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = NinthTrendBasedFibonacciTimePercent,
                    Style = NinthTrendBasedFibonacciTimeStyle,
                    Thickness = NinthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(NinthTrendBasedFibonacciTimeColor, NinthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowTenthTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = TenthTrendBasedFibonacciTimePercent,
                    Style = TenthTrendBasedFibonacciTimeStyle,
                    Thickness = TenthTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(TenthTrendBasedFibonacciTimeColor, TenthTrendBasedFibonacciTimeAlpha),
                });
            }

            if (ShowEleventhTrendBasedFibonacciTime)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EleventhTrendBasedFibonacciTimePercent,
                    Style = EleventhTrendBasedFibonacciTimeStyle,
                    Thickness = EleventhTrendBasedFibonacciTimeThickness,
                    LineColor = ColorParser.Parse(EleventhTrendBasedFibonacciTimeColor, EleventhTrendBasedFibonacciTimeAlpha),
                });
            }

            return result;
        }

        private IEnumerable<Patterns.FibonacciLevel> GetFibonacciChannelLevels()
        {
            var result = new List<Patterns.FibonacciLevel>();

            if (ShowFirstFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FirstFibonacciChannelPercent,
                    Style = FirstFibonacciChannelStyle,
                    Thickness = FirstFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(FirstFibonacciChannelColor, FirstFibonacciChannelAlpha),
                    IsFilled = FillFirstFibonacciChannel,
                });
            }

            if (ShowSecondFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SecondFibonacciChannelPercent,
                    Style = SecondFibonacciChannelStyle,
                    Thickness = SecondFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(SecondFibonacciChannelColor, SecondFibonacciChannelAlpha),
                    IsFilled = FillSecondFibonacciChannel,
                });
            }

            if (ShowThirdFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = ThirdFibonacciChannelPercent,
                    Style = ThirdFibonacciChannelStyle,
                    Thickness = ThirdFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(ThirdFibonacciChannelColor, ThirdFibonacciChannelAlpha),
                    IsFilled = FillThirdFibonacciChannel,
                });
            }

            if (ShowFourthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FourthFibonacciChannelPercent,
                    Style = FourthFibonacciChannelStyle,
                    Thickness = FourthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(FourthFibonacciChannelColor, FourthFibonacciChannelAlpha),
                    IsFilled = FillFourthFibonacciChannel,
                });
            }

            if (ShowFifthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = FifthFibonacciChannelPercent,
                    Style = FifthFibonacciChannelStyle,
                    Thickness = FifthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(FifthFibonacciChannelColor, FifthFibonacciChannelAlpha),
                    IsFilled = FillFifthFibonacciChannel,
                });
            }

            if (ShowSixthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SixthFibonacciChannelPercent,
                    Style = SixthFibonacciChannelStyle,
                    Thickness = SixthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(SixthFibonacciChannelColor, SixthFibonacciChannelAlpha),
                    IsFilled = FillSixthFibonacciChannel,
                });
            }

            if (ShowSeventhFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = SeventhFibonacciChannelPercent,
                    Style = SeventhFibonacciChannelStyle,
                    Thickness = SeventhFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(SeventhFibonacciChannelColor, SeventhFibonacciChannelAlpha),
                    IsFilled = FillSeventhFibonacciChannel,
                });
            }

            if (ShowEighthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EighthFibonacciChannelPercent,
                    Style = EighthFibonacciChannelStyle,
                    Thickness = EighthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(EighthFibonacciChannelColor, EighthFibonacciChannelAlpha),
                    IsFilled = FillEighthFibonacciChannel,
                });
            }

            if (ShowNinthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = NinthFibonacciChannelPercent,
                    Style = NinthFibonacciChannelStyle,
                    Thickness = NinthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(NinthFibonacciChannelColor, NinthFibonacciChannelAlpha),
                    IsFilled = FillNinthFibonacciChannel,
                });
            }

            if (ShowTenthFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = TenthFibonacciChannelPercent,
                    Style = TenthFibonacciChannelStyle,
                    Thickness = TenthFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(TenthFibonacciChannelColor, TenthFibonacciChannelAlpha),
                    IsFilled = FillTenthFibonacciChannel,
                });
            }

            if (ShowEleventhFibonacciChannel)
            {
                result.Add(new Patterns.FibonacciLevel
                {
                    Percent = EleventhFibonacciChannelPercent,
                    Style = EleventhFibonacciChannelStyle,
                    Thickness = EleventhFibonacciChannelThickness,
                    LineColor = ColorParser.Parse(EleventhFibonacciChannelColor, EleventhFibonacciChannelAlpha),
                    IsFilled = FillEleventhFibonacciChannel,
                });
            }

            return result;
        }
    }
}