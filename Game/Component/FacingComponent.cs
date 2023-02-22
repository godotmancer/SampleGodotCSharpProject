namespace SampleGodotCSharpProject.Game.Component;

public partial class FacingComponent : BaseComponent
{
	[Export]
	public Node2D Node2DToFace;

	[Export]
	public VelocityComponent VelocityComponent;

	public override void _PhysicsProcess(double delta)
	{
		if (!Enabled) return;

		if (VelocityComponent != null && Node2DToFace != null)
		{
			Node2DToFace.Rotation = VelocityComponent.Velocity.Normalized().Angle();
		}
	}
}
