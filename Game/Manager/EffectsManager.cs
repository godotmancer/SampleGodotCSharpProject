using System;
using Godot;
using GodotUtilities;

namespace SampleGodotCSharpProject.Game.Manager;

public partial class EffectsManager : Node
{
    private int _currentShakePriority;
    private readonly Random _random = new();
    private static EffectsManager Instance { get; set; }

    public override void _Notification(int what)
    {
        if (what != NotificationEnterTree) return;

        Instance = this;
    }

    private float _RandRange(float min, float max)
    {
        return (float)_random.NextDouble() * (max - min) + min;
    }

    private void _MoveCamera(Vector2 move)
    {
        var camera = GetTree().GetFirstNodeInGroup<Camera2D>();
        camera.Offset = new Vector2(_RandRange(-move.X, move.X), _RandRange(-move.Y, move.Y));
    }

    private void _ShakeScreen(double shakeLength, float shakePower)
    {
        var tweenShake = CreateTween();
        tweenShake.TweenMethod(
                Callable.From<Vector2>(Instance._MoveCamera),
                new Vector2(shakePower, shakePower),
                new Vector2(0, 0),
                shakeLength)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);
    }

    private void _resetCurrentShakePriority()
    {
        _currentShakePriority = 0;
    }

    public static void ShakeScreen(double shakeLength, float shakePower)
    {
        Instance._ShakeScreen(shakeLength, shakePower);
    }
}
