namespace Game.Component;

using Game.Component.Follow;

public partial class FollowPlayerComponent : BaseComponent, IFollowComponent
{
	private PhysicsBody2D _parent;
	private Node2D _player;

	[Export]
	public float Speed = 0.5f;

	[Export]
	public VelocityComponent VelocityComponent;

	public override void _EnterTree()
	{
		this.WireNodes();
		_parent = GetParent<PhysicsBody2D>();
		_player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Enabled) return;

		var playerDir = GlobalPosition.DirectionTo(_player.GlobalPosition);
		var distance = GlobalPosition.DistanceTo(_player.GlobalPosition);
		var newVelocity = playerDir * Speed * distance;
		VelocityComponent.Velocity = newVelocity;
	}
}
