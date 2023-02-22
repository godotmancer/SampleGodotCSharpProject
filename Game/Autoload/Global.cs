using Godot;

namespace SampleGodotCSharpProject.Game.Autoload;

public partial class Global : Node
{
	public Camera2D Camera2D;
	public CanvasLayer Hud;
	public Marker2D ScorePosition;

	public static Global Instance { get; private set; }

	public override void _Notification(int what)
	{
		if (what == NotificationEnterTree) Instance = this;
	}
}