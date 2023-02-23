using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Extension;

namespace SampleGodotCSharpProject.Game.Component.Element;

public partial class FireComponent : ElementComponent
{
	private HealthComponent _healthComponent;
	private Node2D _parent;

	private bool _wait;

	[Node]
	public Area2D CatchFireZone;

	[Node]
	public CollisionShape2D CatchFireZoneCollision;

	[Export]
	public float DepletionRate = -0.03f;

	[Node]
	public Timer DepletionTimer;

	[Export]
	public float EnergyLossPercent = -10.0f;

	[Export]
	public float EnergyTransferPercent = 10.0f;

	[Node]
	public CpuParticles2D FireParticles;

	[Node]
	public Timer HealthDamageTimer;

	[Node]
	public Label Label;

	[Export]
	public float MaxHealthDamage = 0.05f;

	[Node]
	public Node2D Visuals;

	public override void _EnterTree()
	{
		this.WireNodes();
	}

	public override void _Ready()
	{
		DepletionTimer.Timeout += () => { AddEnergy(DepletionRate); };
		HealthDamageTimer.Timeout += _InflictDamage;

		CatchFireZone.BodyEntered += _EnteredCatchFireZone;

		_parent = GetParent() as Node2D;
	}

	private void _EnteredCatchFireZone(Node2D body)
	{
		_ApplyFire(body);
	}

	private void _ApplyFire(Node body)
	{
		// _wait here guards against a race condition whereby getting the first node returns
		// null even though the fire component has been added through a deferred call.
		if (_wait || !Enabled) return;

		var fireComponent = body.GetFirstNodeOfType<FireComponent>();

		if (fireComponent == null)
		{
			_wait = true;
			body.AddResourceDeferredWithAction<FireComponent>(fc =>
			{
				fc.SetEnergy(Energy * EnergyTransferPercent / 100f);
				_wait = false;
			});
		}
		else
		{
			fireComponent.AddEnergy(EnergyTransferPercent / 100f);
			AddEnergy(EnergyLossPercent / 100f);
		}
	}

	public override float AddEnergy(float factor, bool emitSignals = true)
	{
		if (!Enabled) return Energy;

		var oldEnergy = SetEnergy(Energy + factor, emitSignals);
		return oldEnergy;
	}

	public override float SetEnergy(float energy, bool emitSignals = true)
	{
		if (!Enabled) return Energy;

		var result = base.SetEnergy(energy, emitSignals);

		_UpdateVisuals();

		_CheckAndNotify(emitSignals);

		return result;
	}

	private void _UpdateVisuals()
	{
		if (Label.Visible)
			Label.Text = $"{Energy}";
		Visuals.Modulate = new Color(3.0f * Energy, 2.0f * Energy, 2.0f * Energy);
	}

	private void _CheckAndNotify(bool emitSignals)
	{
		if (Energy > 0.0f)
		{
			if (DepletionTimer.IsInsideTree())
			{
				DepletionTimer.Start();
				FireParticles.Emitting = true;
				FireParticles.Lifetime = Energy;
			}
		}
		else
		{
			if (DepletionTimer.IsInsideTree())
			{
				DepletionTimer.Stop();
				FireParticles.Emitting = false;
				if (emitSignals)
				{
					EmitSignal(ElementComponent.SignalName.IntensityDepleted, this);
					GameEvents.EmitElementIntensityDepleted(this);
				}
			}
		}

		if (!(Energy >= 1.0f)) return;

		if (!emitSignals) return;
		EmitSignal(ElementComponent.SignalName.IntensityMaxed, this);
		GameEvents.EmitElementIntensityMaxed(this);
	}

	protected override void _EnabledPostProcess()
	{
		base._EnabledPostProcess();
		if (Enabled) DepletionTimer.Start();
		else DepletionTimer.Stop();
	}

	private void _InflictDamage()
	{
		if (Energy > 0.0f)
		{
			_healthComponent ??= _parent.GetFirstNodeOfType<HealthComponent>();
			_healthComponent?.DecreaseHealth(MaxHealthDamage * Energy);
		}
	}

	public override void _ExitTree()
	{
		if (Enabled)
		{
			EmitSignal(ElementComponent.SignalName.IntensityDepleted, null);
			GameEvents.EmitElementIntensityDepleted(null);
		}
	}
}
