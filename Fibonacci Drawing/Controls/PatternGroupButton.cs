using cAlgo.API;
using cAlgo.Patterns;
using System.Collections.Generic;

namespace cAlgo.Controls
{
    public class PatternGroupButton : ToggleButton
    {
        private readonly StackPanel _panel;

        private readonly List<PatternButton> _currentButtons = new List<PatternButton>();

        public PatternGroupButton(StackPanel panel)
        {
            if (panel == null) throw new System.ArgumentNullException("panel");

            _panel = panel;
        }

        public IEnumerable<IPattern> Patterns { get; set; }

        protected override void OnTurnedOn()
        {
            if (Patterns == null) return;

            RemoveButtons();

            foreach (var pattern in Patterns)
            {
                var button = new PatternButton(pattern)
                {
                    Style = Style,
                    OnColor = OnColor,
                    OffColor = OffColor
                };

                _currentButtons.Add(button);

                _panel.AddChild(button);
            }

            _panel.IsVisible = true;
        }

        protected override void OnTurnedOff()
        {
            RemoveButtons();

            _panel.IsVisible = false;
        }

        private void RemoveButtons()
        {
            if (_currentButtons.Count == 0) return;

            var buttons = _currentButtons.ToArray();

            foreach (var button in buttons)
            {
                _panel.RemoveChild(button);
            }

            _currentButtons.Clear();
        }
    }
}