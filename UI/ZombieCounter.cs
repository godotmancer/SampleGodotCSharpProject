using Godot;
using SampleGodotCSharpProject.Game.Autoload;

namespace SampleGodotCSharpProject.UI;

public partial class ZombieCounter : Label
{
	private int _totalZombies;
	
	public override void _Ready()
	{
		GameEvents.Instance.EnemySpawned += _ =>
		{
			_totalZombies += 1;
			Text = $"Zombies: {_totalZombies}";
		};
	}

}