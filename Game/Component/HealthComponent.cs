using Godot;
using GodotUtilities;

namespace SampleGodotCSharpProject.Game.Component;

public partial class HealthComponent : BaseComponent
{
	[Export]
	public float Health;

	[Export]
	public Gradient HealthGradient;

	[Node]
	public Label Label;

	public override void _EnterTree()
	{
		this.WireNodes();
	}

	public override void _Ready()
	{
		_UpdateVisuals();
	}

	private void _UpdateVisuals()
	{
		if (Label.Visible)
			Label.Text = $"{Health}";
	}

	public float DecreaseHealth(float damage)
	{
		var oldHealth = Health;
		Health -= damage;
		_UpdateVisuals();

		return oldHealth;
	}

	public float IncreaseHealth(float damage)
	{
		var oldHealth = Health;
		Health += damage;
		_UpdateVisuals();

		return oldHealth;
	}

	public void SetHealth(float health)
	{
		Health = health;
		_UpdateVisuals();
	}
}
