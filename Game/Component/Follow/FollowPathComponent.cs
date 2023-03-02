using Game.Component.Follow;

namespace Game.Component;

public partial class FollowPathComponent : BaseComponent, IFollowComponent
{
	[Export]
	public float FollowSpeed = 0.5f;

	[Export]
	public float PathSpeed = 0.1f;

	[Export]
	public VelocityComponent VelocityComponent;

	[Node]
	public PathFollow2D PathFollow2D;

	private Node2D _parent;

	public override void _EnterTree()
	{
		this.WireNodes();
		_parent = GetParent<Node2D>();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Enabled) return;

		PathFollow2D.ProgressRatio += PathSpeed * (float)delta;

		var pathDir = _parent.GlobalPosition.DirectionTo(PathFollow2D.GlobalPosition);
		var distance = _parent.GlobalPosition.DistanceTo(PathFollow2D.GlobalPosition);
		var newVelocity = pathDir * FollowSpeed * distance;
		VelocityComponent.Velocity = newVelocity;
	}
}
