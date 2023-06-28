namespace Game.Component;

using Game.Autoload;
using Game.Component;
using Game.Extension;
using Vector2 = Godot.Vector2;

public partial class ScoreAttractorComponent : BaseComponent
{
	[Export]
	public Node2D AttractorNode;

	[Export]
	public float Duration = 2.0f;

	private float Amount { get; set; }

	private Node2D _node;
	private Vector2 _nodeInitialGlobalPos;

	public override void _Ready()
	{
		_node = GetParent<Node2D>();
		_nodeInitialGlobalPos = _node.GlobalPosition;

		_node.EnableComponent<FacingComponent>(false);
		_node.EnableComponent<FollowPlayerComponent>(false);
		_node.EnableComponent<VelocityComponent>(false);

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
				Node2D.PropertyName.Rotation.ToString(),
				Mathf.Pi * 7.0,
				Duration)
			.SetTrans(Tween.TransitionType.Expo)
			.SetEase(Tween.EaseType.In);

		tween.TweenCallback(
			Callable.From(() =>
			{
				var hitDirection = AttractorNode.GlobalPosition.DirectionTo(_node.GlobalPosition);
				var hitAngle = AttractorNode.GlobalPosition.AngleToPoint(_node.GlobalPosition);
				GameEvents.EmitPlayerHit(AttractorNode, hitAngle, hitDirection);
			}));

		tween.TweenCallback(Callable.From(QueueFree));
	}

	public override void _PhysicsProcess(double delta)
	{
		var fireballPos = AttractorNode.GlobalPosition;
		var distance = _nodeInitialGlobalPos.DistanceTo(fireballPos) * Amount;
		_node.GlobalPosition = _nodeInitialGlobalPos.MoveToward(fireballPos, distance);
	}
}
