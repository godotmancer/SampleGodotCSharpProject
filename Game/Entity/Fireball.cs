using Godot;
using Godot.Collections;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Component;
using SampleGodotCSharpProject.Game.Component.Element;
using SampleGodotCSharpProject.Game.Extension;

namespace SampleGodotCSharpProject.Game.Entity;

public partial class Fireball : StaticBody2D
{
	private Dictionary<string, FireComponent> _nodesInsideHotZone = new();
	private Vector2 _screenSize;

	[Node]
	public FollowMouseComponent FollowMouseComponent;

	[Export]
	public float HeatUpEnergyRate = 0.075f;

	[Export]
	public bool HeatUpTouching = true;

	[Node]
	public Area2D HotZone;

	[Node]
	public Sprite2D Skull;

	[Node]
	public Timer Timer;

	private Vector2 _skullScale;

	public override void _EnterTree()
	{
		this.WireNodes();
	}

	public override async void _Ready()
	{
		_screenSize = GetViewportRect().Size;

		HotZone.BodyEntered += _EnteredHotZone;
		HotZone.BodyExited += _ExitedHotZone;
		Timer.Timeout += _HeatUpHotZone;
		GameEvents.Instance.PlayerHit += _hit;

		_skullScale = Skull.Scale;

		await ToSignal(GetTree(), "process_frame");

		Input.WarpMouse(_screenSize / 2.0f);
	}

	public override void _PhysicsProcess(double delta)
	{
		FollowMouseComponent.Follow(delta);
	}

	private void _EnteredHotZone(Node2D body)
	{
		var fireComponent = body.GetFirstNodeOfType<FireComponent>() ?? body.AddResourceDeferred<FireComponent>();

		_nodesInsideHotZone.Add(body.Name, fireComponent);
	}

	private void _ExitedHotZone(Node2D body)
	{
		_nodesInsideHotZone.Remove(body.Name);
	}

	private void _HeatUpHotZone()
	{
		if (!HeatUpTouching) return;
		foreach (var fireComponent in _nodesInsideHotZone.Values)
		{
			fireComponent.AddEnergy(HeatUpEnergyRate);
		}
	}

	private void _hit(Node2D player)
	{
		var tween = CreateTween();
		tween.TweenProperty(
				Skull,
				"modulate",
				Colors.White,
				0.25f)
			.From(this.IntensifyColor(Colors.Magenta, 2.3f))
			.SetTrans(Tween.TransitionType.Linear)
			.SetEase(Tween.EaseType.In);
		tween.Parallel().TweenProperty(
				Skull,
				"scale",
				_skullScale,
				0.25f)
			.From(_skullScale * 1.3f)
			.SetTrans(Tween.TransitionType.Linear)
			.SetEase(Tween.EaseType.In);
	}
}