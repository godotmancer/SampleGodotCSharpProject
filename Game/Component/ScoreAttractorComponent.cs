using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Entity;

namespace SampleGodotCSharpProject.Game.Component
{
    public partial class ScoreAttractorComponent : BaseComponent
    {
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
                    1.5f)
                .SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.In);
            tween.Parallel()
                .TweenProperty(
                    _node,
                    "modulate",
                    Colors.Transparent,
                    2.5f)
                .SetTrans(Tween.TransitionType.Expo)
                .SetEase(Tween.EaseType.In);
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
