namespace Game.Manager;

public partial class FxManager : Node
{
	public static FxManager Instance { get; private set; }

	private readonly Random _random = new();
	private int _currentShakePriority;

	public static void ShakeScreen(double shakeLength, float shakePower) =>
		Instance._ShakeScreen(shakeLength, shakePower);

	public override void _Notification(int what)
	{
		if (what != NotificationEnterTree) return;

		Instance = this;
	}

	private float _RandRange(float min, float max) =>
		(float)_random.NextDouble() * (max - min) + min;

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

	private void _resetCurrentShakePriority() => _currentShakePriority = 0;
}
