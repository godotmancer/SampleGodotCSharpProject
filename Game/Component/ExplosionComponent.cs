using Godot;
using GodotUtilities;
using SampleGodotCSharpProject.Game.Manager;
using SampleGodotCSharpProject.Game.Extension;
using Zombie = SampleGodotCSharpProject.Game.Entity.Enemy.Zombie;

namespace SampleGodotCSharpProject.Game.Component;

public partial class ExplosionComponent : BaseComponent
{
    [Node]
    public AnimatedSprite2D AnimatedSprite2D;

    [Node]
    public Node2D Visuals;

    public override void _EnterTree()
    {
        this.WireNodes();
    }

    public override void _Ready()
    {
        AnimatedSprite2D.AnimationFinished += QueueFree;
     
        Visuals.Rotation = (float)GD.Randfn(Mathf.Pi*2, Mathf.Pi);

        if (GetParent() is Zombie node)
        {
            node.Modulate = this.IntensifyColor(Colors.DarkMagenta, 2.3f);
            node.GetFirstNodeOfType<FacingComponent>()?.SetEnabled(false);
            node.GetFirstNodeOfType<FollowPlayerComponent>()?.SetEnabled(false);
            var velocityComponent = node.GetFirstNodeOfType<VelocityComponent>();
            if (velocityComponent != null)
            {
                velocityComponent.DisableCollisionCheck(true);
                // velocityComponent.ApplyGravity(node, new Vector2(0, 9.8f));
            }
        }
        
        var shakeIntensity = _CalcShakeIntensity();
        if (shakeIntensity <= float.Epsilon) return;
        
        EffectsManager.ShakeScreen(1f, 5*shakeIntensity);
    }

    private float _CalcShakeIntensity()
    {
        var camera = GetTree().GetFirstNodeInGroup<Camera2D>();
        var shakeIntensity = Mathf.Remap(GlobalPosition.DistanceTo(camera.GlobalPosition), 0, 500f, 1.0f, 0.0f);
        return shakeIntensity;
    }
}