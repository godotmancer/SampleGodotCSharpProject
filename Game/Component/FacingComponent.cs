namespace Game.Component;

public partial class FacingComponent : BaseComponent
{
	[Export]
	public Node2D Node2DToRotate;

	[Export]
	public VelocityComponent VelocityComponent;

	public void Update(double delta)
	{
		if (!Enabled) return;

		Node2DToRotate.Rotation = VelocityComponent.Velocity.Normalized().Angle();
	}
}
