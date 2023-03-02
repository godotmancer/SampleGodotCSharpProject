namespace Game.Component;

public partial class FacingComponent : BaseComponent
{
	[Export]
	public Node2D Node2DToRotate;

	[Export]
	public VelocityComponent VelocityComponent;

	public override void _PhysicsProcess(double delta)
	{
		if (!Enabled) return;

		Node2DToRotate.Rotation = VelocityComponent.Velocity.Normalized().Angle();
	}
}
