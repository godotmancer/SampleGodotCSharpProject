using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Extension;

namespace SampleGodotCSharpProject.Game.Component
{
    public partial class ScoreAttractorComponent : BaseComponent
    {
        [Export]
        public Node2D AttractorNode;

        [Export]
        public float Duration = 2.0f;

        private Node2D _node;
        private Vector2 _nodeInitialGlobalPos;
        private float Amount { get; set; }

        public override void _Ready()
        {
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
            
            tween.TweenCallback(
                Callable.From(() => GameEvents.EmitPlayerHit(AttractorNode)));

            tween.TweenCallback(Callable.From(QueueFree));
        }

        public override void _PhysicsProcess(double delta)
        {
            var fireballPos = AttractorNode.GlobalPosition;
            _node.GlobalPosition = _nodeInitialGlobalPos
                .MoveToward(
                    fireballPos,
                    _nodeInitialGlobalPos.DistanceTo(fireballPos) * Amount);
        }
    }
}
