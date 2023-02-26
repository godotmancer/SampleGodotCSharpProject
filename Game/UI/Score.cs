using Game.Autoload;

namespace Game.UI;

public partial class Score : Label
{
	private int _totalScore;
	private Vector2 _pivot;

	public override void _Ready()
	{
		_pivot = Size / 2.0f;

		GameEvents.Instance.PlayerHit += (_, _, _) =>
		{
			_totalScore += 1;
			Text = $"Score: {_totalScore}";

			_pivot.X = Size.X;
			PivotOffset = _pivot;

			var tween = CreateTween();
			tween.TweenProperty(
					this,
					Node2D.PropertyName.Scale.ToString(),
					Vector2.One,
					0.25f)
				.From(Vector2.One * 1.8f);
		};
	}
}
