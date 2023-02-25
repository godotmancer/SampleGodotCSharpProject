using Game.Entity.Enemy;
using Game.Manager;
using Game.Extension;

namespace Game.Component;

public partial class ExplosionComponent : BaseComponent
{
	[Node]
	public AnimatedSprite2D AnimatedSprite2D;

	[Node]
	public Node2D Visuals;

	public override void _EnterTree()
	{
		this.WireNodes();
	}

	public override void _Ready()
	{
		AnimatedSprite2D.AnimationFinished += QueueFree;

		Visuals.Rotation = (float)GD.Randfn(Mathf.Pi * 2, Mathf.Pi);

		if (GetParent() is Zombie node)
		{
			node.ZIndex = 2;
			node.Modulate = this.IntensifyColor(Colors.DarkMagenta, 2.3f);
			node.GetFirstNodeOfType<FacingComponent>()?.SetEnabled(false);
			node.GetFirstNodeOfType<FollowPlayerComponent>()?.SetEnabled(false);
			node.GetFirstNodeOfType<VelocityComponent>()?.SetEnabled(false);
		}

		var shakeIntensity = _CalcShakeIntensity();
		if (shakeIntensity <= float.Epsilon) return;

		FxManager.ShakeScreen(1f, 5 * shakeIntensity);
	}

	private float _CalcShakeIntensity()
	{
		var camera = GetTree().GetFirstNodeInGroup<Camera2D>();
		var shakeIntensity = Mathf.Remap(GlobalPosition.DistanceTo(camera.GlobalPosition), 0, 500f, 1.0f, 0.0f);
		return shakeIntensity;
	}
}
