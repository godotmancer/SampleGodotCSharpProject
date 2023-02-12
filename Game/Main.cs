using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Entity;
using SampleGodotCSharpProject.Helpers;

namespace SampleGodotCSharpProject.Game;

public partial class Main : Node2D
{
    [Export]
    public int TotalEnemies = 500;

    [Export]
    public PackedScene EnemyScene;

    [Node]
    public Node2D Fireball;

    [Node]
    public CanvasGroup Enemies;

    private RandomNumberGenerator _random = new();
    private Rect2 _rect;
    private Vector2 _screenSize;
    private int _killedEnemies;
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

        _CreateEnemies();

        GameEvents.Instance.EnemyKilled += _ =>
        {
            _killedEnemies++;
            if (_killedEnemies % _random.RandiRange(2, 6) == 0)
            {
                _CreateEnemy(_pointGenerator.GeneratePoint());
            }
        };
    }

    private void _CreateEnemies()
    {
        var points = _pointGenerator.GeneratePoints(TotalEnemies);
        foreach (var point in points)
        {
            _CreateEnemy(point);
        }
    }

    private void _CreateEnemy(Vector2 point)
    {
        var enemy = EnemyScene.Instantiate<Enemy>();
        enemy.GlobalPosition = point;

        Enemies.AddChild(enemy);
    }
}
