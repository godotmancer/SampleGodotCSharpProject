using Godot;
using GodotUtilities;

namespace SampleGodotCSharpProject.Game.Component;

public partial class FollowPlayerComponent : BaseComponent
{
    [Export]
    public VelocityComponent VelocityComponent;

    [Export]
    public float Speed = 0.5f;

    private PhysicsBody2D _parent;
    private Node2D _player;

    public override void _EnterTree()
    {
        this.WireNodes();
        _parent = GetParent<PhysicsBody2D>();
        _player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
    }

    public void Follow(double delta)
    {
        if (!Enabled) return;
        
        var playerDir = GlobalPosition.DirectionTo(_player.GlobalPosition);
        var distance = GlobalPosition.DistanceTo(_player.GlobalPosition);
        var newVelocity = playerDir * Speed * distance;
        VelocityComponent.Velocity = newVelocity;
        VelocityComponent.MoveAndCollide(_parent, delta);
    }
}
