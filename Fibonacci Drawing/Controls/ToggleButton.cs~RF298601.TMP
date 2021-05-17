using cAlgo.API;

namespace cAlgo.Controls
{
    public class ToggleButton : Button
    {
        private Color _onColor;
        private Color _offColor;

        public Color OnColor
        {
            get { return _onColor; }
            set
            {
                _onColor = value;

                if (IsOn) BackgroundColor = OnColor;
            }
        }

        public Color OffColor
        {
            get { return _offColor; }
            set
            {
                _offColor = value;

                if (!IsOn) BackgroundColor = _offColor;
            }
        }

        public bool IsOn { get; private set; }

        public void TurnOn()
        {
            IsOn = true;

            BackgroundColor = OnColor;

            OnTurnedOn();
        }

        public void TurnOff()
        {
            IsOn = false;

            BackgroundColor = OffColor;

            OnTurnedOff();
        }

        protected virtual void OnTurnedOn()
        {
        }

        protected virtual void OnTurnedOff()
        {
        }
    }
}