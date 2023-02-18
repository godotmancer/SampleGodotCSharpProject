using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Entity;
using SampleGodotCSharpProject.Game.Extension;
using SampleGodotCSharpProject.Game.Manager;

namespace SampleGodotCSharpProject.Game.Component
{
    public partial class ScoreAttractorComponent : BaseComponent
    {
        [Export]
        public float Duration = 2.0f;

        private Node2D _fireball;
        private Node2D _node;
        private Vector2 _nodeInitialGlobalPos;
        private float Amount { get; set; }

        public override void _Ready()
        {
            _fireball = GetTree().GetFirstNodeInGroup<Fireball>();
            _node = (Node2D)GetParent();
            _nodeInitialGlobalPos = _node.GlobalPosition;

            var tween = CreateTween();
            tween.TweenProperty(
                    this,
                    nameof(Amount),
                    1.0f,
                    Duration)
                .SetTrans(Tween.TransitionType.Back)
                .SetEase(Tween.EaseType.In);
            tween.Parallel().TweenProperty(
                    _node,
                    "rotation",
                    Mathf.Pi * 7.0,
                    Duration)
                .SetTrans(Tween.TransitionType.Expo)
                .SetEase(Tween.EaseType.In);

            tween.TweenProperty(
                    _fireball,
                    "modulate",
                    Colors.White,
                    0.25f)
                .From(this.IntensifyColor(Colors.Magenta, 2.3f))
                .SetTrans(Tween.TransitionType.Linear)
                .SetEase(Tween.EaseType.In);
            tween.Parallel().TweenProperty(
                    _fireball,
                    "scale",
                    Vector2.One,
                    0.25f)
                .From(Vector2.One * 1.2f)
                .SetTrans(Tween.TransitionType.Linear)
                .SetEase(Tween.EaseType.In);
            tween.Parallel().TweenCallback(Callable.From(GameEvents.EmitPlayerHit));

            tween.TweenCallback(Callable.From(QueueFree));
        }

        public override void _PhysicsProcess(double delta)
        {
            var fireballPos = _fireball.GlobalPosition;
            _node.GlobalPosition = _nodeInitialGlobalPos
                .MoveToward(
                    fireballPos,
                    _nodeInitialGlobalPos.DistanceTo(fireballPos) * Amount);
        }
    }
}
