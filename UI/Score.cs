using Godot;
using SampleGodotCSharpProject.Game.Autoload;

namespace SampleGodotCSharpProject.UI
{
    public partial class Score : Label
    {
        private int _totalScore;

        public override void _Ready()
        {
            GameEvents.Instance.ZombieKilled += _ =>
            {
                _totalScore += 1;
                Text = $"Score: {_totalScore}";
            };
        }
    }
}
