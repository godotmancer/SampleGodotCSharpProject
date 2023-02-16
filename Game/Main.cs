using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Entity;
using SampleGodotCSharpProject.Helpers;
using Zombie = SampleGodotCSharpProject.Game.Entity.Enemy.Zombie;

namespace SampleGodotCSharpProject.Game;

public partial class Main : Node2D
{
    [Export]
    public int TotalZombies = 500;

    [Export]
    public PackedScene ZombieScene;

    [Node]
    public Node2D Fireball;

    [Node]
    public CanvasGroup Zombies;

    private RandomNumberGenerator _random = new();
    private Rect2 _rect;
    private Vector2 _screenSize;
    private int _killedZombies;
    private PointGenerator _pointGenerator;

    public override void _EnterTree()
    {
        this.WireNodes();
    }

    public override void _Ready()
    {
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
            if (_killedZombies % _random.RandiRange(2, 6) == 0)
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
