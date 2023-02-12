using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Extension;

namespace SampleGodotCSharpProject.Game.Component.Element;

public partial class FireComponent : ElementComponent
{
    [Export]
    public bool DisableCatchFire
    {
        set => CatchFireZoneCollision.CallDeferred("set_disabled", value);
        get => CatchFireZoneCollision.Disabled;
    }

    [Export]
    public float DepletionRate = -0.03f;

    [Export]
    public float EnergyTransferPercent = 10.0f;

    [Export]
    public float EnergyLossPercent = -10.0f;

    [Node]
    public CpuParticles2D FireParticles;

    [Node]
    public Label Label;

    [Node]
    public Timer DepletionTimer;

    [Node]
    public Area2D CatchFireZone;

    [Node]
    public CollisionShape2D CatchFireZoneCollision;

    [Node]
    public Node2D Visuals;

    private bool _wait;

    public override void _EnterTree()
    {
        this.WireNodes();
    }

    public override void _Ready()
    {
        DepletionTimer.Start();
        DepletionTimer.Timeout += () => { AddEnergy(DepletionRate); };

        CatchFireZone.BodyEntered += _EnteredCatchFireZone;
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
            body.InstantiateChildDeferredWithAction<FireComponent>((fc) =>
            {
                fc.SetEnergy(Energy * EnergyTransferPercent / 100f, false);
                _wait = false;
            });
        }
        else
        {
            fireComponent.AddEnergy(EnergyTransferPercent / 100f, false);
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
            DepletionTimer.Start();
            FireParticles.Emitting = true;
            FireParticles.Lifetime = Energy;
        }
        else
        {
            DepletionTimer.Stop();
            FireParticles.Emitting = false;
            if (emitSignals)
            {
                EmitSignal(ElementComponent.SignalName.IntensityDepleted, this);
                GameEvents.EmitElementIntensityDepleted(this);
            }
        }

        if (!(Energy >= 1.0f)) return;

        if (GetParent() is Node2D parent)
        {
            _ExplodeEntity(parent);
        }

        if (!emitSignals) return;
        EmitSignal(ElementComponent.SignalName.IntensityMaxed, this);
        GameEvents.EmitElementIntensityMaxed(this);
    }

    private void _ExplodeEntity(Node2D node)
    {
        node.AddChild(this.InstantiateFromResources<ExplosionComponent>());

        DisableCatchFire = true;
    }

    protected override void _EnabledPostProcess()
    {
        base._EnabledPostProcess();
        if (Enabled) DepletionTimer.Start();
        else DepletionTimer.Stop();
    }
}
