using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Entity.Enemy;
using SampleGodotCSharpProject.Helpers;

namespace SampleGodotCSharpProject.Game;

public partial class Main : Node2D
{
	private int _killedZombies;
	private PointGenerator _pointGenerator;
	private RandomNumberGenerator _random = new();
	private Rect2 _rect;
	private Vector2 _screenSize;

	[Node]
	public Camera2D Camera2D;

	[Node]
	public Node2D Fireball;

	[Node]
	public CanvasLayer Hud;

	[Export]
	public int TotalZombies = 500;

	[Node]
	public CanvasGroup Zombies;

	[Export]
	public PackedScene ZombieScene;

	public override void _EnterTree()
	{
		this.WireNodes();
	}

	public override void _Ready()
	{
		Global.Instance.Hud = Hud;
		Global.Instance.Camera2D = Camera2D;

		_random.Seed = 1234L;
		_screenSize = GetViewportRect().Size;

		Fireball.GlobalPosition = _screenSize / 2;

		_pointGenerator = new PointGenerator(
			_screenSize / 2.0f,
			500,
			200);

		_CreateZombies();

		GameEvents.Instance.ZombieKilled += _ =>
		{
			_killedZombies++;
			if (_killedZombies % _random.RandiRange(1, 2) == 0)
			{
				_CreateZombies(_pointGenerator.GeneratePoint());
			}
		};
	}

	private void _CreateZombies()
	{
		var points = _pointGenerator.GeneratePoints(TotalZombies);
		foreach (var point in points)
		{
			_CreateZombies(point);
		}
	}

	private void _CreateZombies(Vector2 point)
	{
		var zombie = ZombieScene.Instantiate<Zombie>();
		zombie.GlobalPosition = point;

		Zombies.AddChild(zombie);
	}
}
