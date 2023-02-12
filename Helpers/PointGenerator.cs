using Godot;

namespace SampleGodotCSharpProject.Helpers;

class PointGenerator
{
    private readonly Vector2 _center;
    private readonly float _radius;
    private readonly float _padding;
    private readonly RandomNumberGenerator _random = new();

    public PointGenerator(Vector2 center, float radius, float padding)
    {
        _center = center;
        _radius = radius;
        _padding = padding;

        _random.Seed = 1234L;

    }

    public Vector2[] GeneratePoints(int totalPoints)
    {
        var points = new Vector2[totalPoints];

        for (var i = 0; i < totalPoints; i++)
        {
            var point = GeneratePoint();
            points[i] = point;
        }

        return points;
    }

    public Vector2 GeneratePoint()
    {
        var radius = _radius + (_padding * _random.RandfRange(0.1f, 3.0f));
        var angle = _random.RandfRange(0.0f, 720.0f);
        var x = _center.X + radius * Mathf.Cos(Mathf.DegToRad(angle));
        var y = _center.Y + radius * Mathf.Sin(Mathf.DegToRad(angle));
        return new Vector2(x, y);
    }
}
