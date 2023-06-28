namespace Game.Component;

using Game.Autoload;
using Game.Entity.Enemy;
using Game.Manager;
using Game.Extension;

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

		Visuals.Rotation = (float)GD.Randfn(Mathf.Tau, Mathf.Pi);

		if (GetParent() is Zombie node)
		{
			node.ZIndex = 2;
			node.Modulate = this.IntensifyColor(Colors.DarkMagenta, 2.3f);
		}

		var shakeIntensity = _CalcShakeIntensity();
		if (shakeIntensity <= float.Epsilon) return;

		FxManager.ShakeScreen(1f, 5 * shakeIntensity);
	}

	private float _CalcShakeIntensity()
	{
		var shakeIntensity = Mathf.Remap(GlobalPosition.DistanceTo(Global.Instance.Camera2D.GlobalPosition), 0, 500f, 1.0f, 0.0f);
		return shakeIntensity;
	}
}
