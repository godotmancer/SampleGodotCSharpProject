using Godot;
using GodotUtilities;
using GodotUtilities.Logic;
using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Component;

namespace SampleGodotCSharpProject.Game.Entity;

public partial class Zombie : CharacterBody2D
{
    [Node]
    public VelocityComponent VelocityComponent;

    [Node]
    public FollowPlayerComponent FollowPlayerComponent;

    [Node]
    public AnimationPlayer AnimationPlayer;

    [Node]
    public CollisionShape2D CollisionShape2D;

    [Node]
    public Node2D Visuals;

    [Node]
    public AnimatedSprite2D AnimatedSprite2D;

    private DelegateStateMachine _stateMachine = new();

    public override void _EnterTree()
    {
        this.WireNodes();
    }

    public override void _Ready()
    {
        // Entity Initialisations
        Scale *= (float)GD.RandRange(0.1, 0.4);

        // State Machine States
        _stateMachine.AddStates(StateIdle, EnterStateIdle);
        _stateMachine.AddStates(StateWalk, EnterStateWalk);
        _stateMachine.AddStates(StateStopped, EnterStateStopped);
        _stateMachine.SetInitialState(StateIdle);
        
        GameEvents.EmitSpawnEnemy(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        _stateMachine.Update();
    }

    private void _CheckSpeed()
    {
        var speed = VelocityComponent.Speed;
        if (speed <= 0.1f)
        {
            if (_stateMachine.GetCurrentState() != StateIdle)
            {
                _stateMachine.ChangeState(StateIdle);
                return;
            }
        }
        else
        {
            if (_stateMachine.GetCurrentState() != StateWalk)
            {
                _stateMachine.ChangeState(StateWalk);
            }
        }

        if (!(VelocityComponent.Velocity.Y > 1000.0f)) return;

        GameEvents.EmitEnemyKilled(this);
        QueueFree();

    }

    private void EnterStateIdle()
    {
        AnimatedSprite2D.Pause();
    }

    private void StateIdle()
    {
        FollowPlayerComponent?.Follow(GetPhysicsProcessDeltaTime());
        _CheckSpeed();
    }

    private void EnterStateWalk()
    {
        AnimatedSprite2D.Play("walk");
    }

    private void StateWalk()
    {
        FollowPlayerComponent?.Follow(GetPhysicsProcessDeltaTime());
        _CheckSpeed();
    }

    private void EnterStateStopped()
    {
        AnimationPlayer.Play("RESET");
    }

    private void StateStopped()
    {
        VelocityComponent.MoveAndCollide(this, GetPhysicsProcessDeltaTime());
        _CheckSpeed();
    }
}
