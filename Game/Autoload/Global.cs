global using Godot;
global using GodotUtilities;
global using GodotUtilities.Logic;
global using System;

namespace Game.Autoload;

public partial class Global : Node
{
	public Camera2D Camera2D;
	public CanvasLayer Hud;

	public static Global Instance { get; private set; }

	public override void _Notification(int what)
	{
		if (what == NotificationEnterTree) Instance = this;
	}
}
