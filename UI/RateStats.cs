using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;

namespace SampleGodotCSharpProject.UI
{
    public partial class RateStats : Control
    {
        [Node]
        public Timer Timer;

        [Node]
        public Label HitRate;

        [Node]
        public Label MaxRate;

        private int _counter;
        private int _rate;
        private int _maxRate;

        public override void _EnterTree()
        {
            this.WireNodes();
        }

        public override void _Ready()
        {
            Timer.Timeout += () =>
            {
                _rate = _counter;
                _counter = 0;
                HitRate.Text = $"Hits: {_rate:D0}/s";
            };
            GameEvents.Instance.PlayerHit += _ =>
            {
                _counter += 1;
                if (_maxRate <= _counter)
                {
                    _maxRate = _counter;
                    MaxRate.Text = $"Max: {_maxRate:D0}";
                }
            };
        }
    }
}
