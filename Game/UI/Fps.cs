namespace Game.UI;

public partial class Fps : Label
{
	public override void _PhysicsProcess(double delta)
	{
		Text = $"{Engine.GetFramesPerSecond()}/s";
	}
}
