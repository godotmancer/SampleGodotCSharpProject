using Godot;
using GodotUtilities;

namespace SampleGodotCSharpProject.Game.Component;

public partial class FollowMouseComponent : BaseComponent
{
    [Export]
    public VelocityComponent VelocityComponent;

    [Export]
    public float Speed = 30.0f;

    private PhysicsBody2D _parent;

    public override void _EnterTree()
    {
        this.WireNodes();
        _parent = GetParent<PhysicsBody2D>();
    }

    public void Follow(double delta)
    {
        if (!Enabled) return;
        
        var mouseDir = this.GetMouseDirection();
        var distance = GlobalPosition.DistanceTo(GetGlobalMousePosition());
        var newVelocity = mouseDir * Speed * distance;
        VelocityComponent.Velocity = newVelocity;
        VelocityComponent.MoveAndCollide(_parent, delta);
    }
}
