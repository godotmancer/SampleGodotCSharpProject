namespace Game.Component;

public partial class FollowMouseComponent : BaseComponent
{
	private PhysicsBody2D _parent;

	[Export]
	public float Speed = 30.0f;

	[Export]
	public VelocityComponent VelocityComponent;

	public override void _EnterTree()
	{
		this.WireNodes();
		_parent = GetParent<PhysicsBody2D>();
	}

	public void Follow(double delta)
	{
		if (!Enabled) return;

		var mouseDir = this.GetMouseDirection();
		var distance = GlobalPosition.DistanceTo(GetGlobalMousePosition());
		var newVelocity = mouseDir * Speed * distance;
		VelocityComponent.Velocity = newVelocity;
	}
}
