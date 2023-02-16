using Godot;
using Godot.Collections;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Component;
using SampleGodotCSharpProject.Game.Component.Element;
using SampleGodotCSharpProject.Game.Extension;

namespace SampleGodotCSharpProject.Game.Entity;

public partial class Fireball : StaticBody2D
{
    [Export]
    public bool HeatUpTouching = true;

    [Export]
    public float HeatUpEnergyRate = 0.075f;

    [Node]
    public FollowMouseComponent FollowMouseComponent;

    [Node]
    public Area2D HotZone;

    [Node]
    public Timer Timer;

    [Node]
    public Sprite2D Skull;

    private Dictionary<string, FireComponent> _nodesInsideHotZone = new();
    private Vector2 _screenSize;

    public override void _EnterTree()
    {
        this.WireNodes();
    }

    public override async void _Ready()
    {
        _screenSize = GetViewportRect().Size;

        HotZone.BodyEntered += _EnteredHotZone;
        HotZone.BodyExited += _ExitedHotZone;
        Timer.Timeout += _HeatUpHotZone;

        await ToSignal(GetTree(), "process_frame");

        Input.WarpMouse(_screenSize/2.0f);
    }

    public override void _PhysicsProcess(double delta)
    {
        FollowMouseComponent.Follow(delta);
    }

    private void _EnteredHotZone(Node2D body)
    {
        var fireComponent = body.GetFirstNodeOfType<FireComponent>();

        if (fireComponent == null)
        {
            fireComponent = this.InstantiateFromResources<FireComponent>();
            body.AddChildDeferred(fireComponent);
        }
        _nodesInsideHotZone.Add(body.Name, fireComponent);
    }

    private void _ExitedHotZone(Node2D body)
    {
        _nodesInsideHotZone.Remove(body.Name);
    }

    private void _HeatUpHotZone()
    {
        if (!HeatUpTouching) return;
        foreach (var fireComponent in _nodesInsideHotZone.Values)
        {
            fireComponent.AddEnergy(HeatUpEnergyRate);
        }
    }
}
