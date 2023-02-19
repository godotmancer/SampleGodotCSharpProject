using System;
using Godot;
using GodotUtilities;
using GodotUtilities.Logic;
using SampleGodotCSharpProject.Game.Autoload;
using SampleGodotCSharpProject.Game.Component;
using SampleGodotCSharpProject.Game.Extension;

namespace SampleGodotCSharpProject.Game.Entity.Enemy
{
    public partial class Zombie : BaseEnemy
    {
        [Node]
        public AnimatedSprite2D AnimatedSprite2D;

        [Node]
        public CollisionShape2D CollisionShape2D;

        [Node]
        public FollowPlayerComponent FollowPlayerComponent;

        [Node]
        public HealthComponent HealthComponent;

        [Node]
        public VelocityComponent VelocityComponent;

        [Node]
        public Node2D Visuals;

        private DelegateStateMachine _stateMachine = new();
        private float _oldHealth;

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
            _stateMachine.SetInitialState(StateIdle);

            GameEvents.EmitSpawnZombie(this);
        }

        public override void _PhysicsProcess(double delta)
        {
            _stateMachine.Update();
        }

        private void _CheckHealth()
        {
            if (HealthComponent != null)
            {
                if (HealthComponent.Health <= 0.0f && !IsDead)
                {
                    IsDead = true;

                    this.AddResourceAndQueueFree<ExplosionComponent>();
                    this.AddResourceAndQueueFree<ScoreAttractorComponent>();

                    GameEvents.EmitZombieKilled(this);
                }
                else if (Math.Abs(HealthComponent.Health - _oldHealth) > 0.001f)
                {
                    _oldHealth = HealthComponent.Health;
                    Visuals.Modulate = HealthComponent.HealthGradient.Sample(1.0f - _oldHealth);
                }
            }
        }

        private void _CheckSpeed()
        {
            var speed = VelocityComponent.Speed;
            if (speed <= 0.1f)
            {
                if (_stateMachine.GetCurrentState() != StateIdle)
                {
                    _stateMachine.ChangeState(StateIdle);
                }
            }
            else
            {
                if (_stateMachine.GetCurrentState() != StateWalk)
                {
                    _stateMachine.ChangeState(StateWalk);
                }
            }
        }

        private void EnterStateIdle()
        {
            AnimatedSprite2D.Pause();
        }

        private void StateIdle()
        {
            FollowPlayerComponent?.Follow(GetPhysicsProcessDeltaTime());

            _CheckHealth();
            _CheckSpeed();
        }

        private void EnterStateWalk()
        {
            AnimatedSprite2D.Play("walk");
        }

        private void StateWalk()
        {
            FollowPlayerComponent?.Follow(GetPhysicsProcessDeltaTime());
            _CheckHealth();
            _CheckSpeed();
        }
    }
}
