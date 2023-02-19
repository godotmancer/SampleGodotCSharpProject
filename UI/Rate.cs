using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;

namespace SampleGodotCSharpProject.UI
{
    public partial class Rate : Label
    {
        [Node]
        public Timer Timer;

        private float _counter;
        private float _rate;

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
                Text = $"Hits: {_rate:F0}/s";
            };
            GameEvents.Instance.PlayerHit += _ =>
            {
                _counter += 1f;
            };
        }
    }
}
