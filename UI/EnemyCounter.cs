using Godot;
using SampleGodotCSharpProject.Game.Autoload;

namespace SampleGodotCSharpProject.UI;

public partial class EnemyCounter : Label
{
	private int _totalEnemies;
	
	public override void _Ready()
	{
		GameEvents.Instance.EnemySpawned += _ =>
		{
			_totalEnemies += 1;
			Text = $"Enemies: {_totalEnemies}";
		};
	}

}