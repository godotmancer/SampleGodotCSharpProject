using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;

namespace SampleGodotCSharpProject.Game.Manager
{
    public partial class SoundManager : Node
    {
        [Export]
        public bool Enabled = true;
        
        [Node]
        public AudioStreamPlayer2D Explosion;

        [Node]
        public AudioStreamPlayer2D PlayerHit;

        [Node]
        public AudioStreamPlayer Fire;

        private float _fireIntensity;
        
        public override void _EnterTree()
        {
            this.WireNodes();
        }

        public override void _Ready()
        {
            GameEvents.Instance.PlayerHit += player =>
            {
                if (!Enabled) return;
                PlayerHit.GlobalPosition = player.GlobalPosition;
                PlayerHit.PlayWithPitch((float)GD.RandRange(0.8f,1.1f));
            };
            
            GameEvents.Instance.ZombieKilled += zombie =>
            {
                if (!Enabled) return;
                Explosion.GlobalPosition = zombie.GlobalPosition;
                Explosion.PlayWithPitch((float)GD.RandRange(0.6f,1.4f));
            };

            GameEvents.Instance.ElementIntensityDepleted += _ =>
            {
                if (!Enabled) return;
                _fireIntensity = Mathf.Max(_fireIntensity - 1.0f, 0.0f);
                if (_fireIntensity <= 0.0f)
                {
                    if (Fire.Playing) Fire.Stop();
                }

            };
            
            GameEvents.Instance.ElementIntensityMaxed += _ =>
            {
                if (!Enabled) return;
                _fireIntensity += 1.0f;
                if (!Fire.Playing) Fire.Play();
            };

        }
    }
}
