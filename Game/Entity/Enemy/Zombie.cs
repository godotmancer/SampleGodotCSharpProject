using Game.Autoload;
using Game.Component;
using Game.Extension;

namespace Game.Entity.Enemy;

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
		Scale *= (float)GD.RandRange(0.15, 0.45);

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

				// TODO: a little verbose - maybe create an extension method???
				var scoreAttractorComponent = this.InstantiateFromResources<ScoreAttractorComponent>();
				scoreAttractorComponent.AttractorNode = GetTree().GetFirstNodeInGroup<Fireball>();
				this.AddNodeToQueueFreeComponent(scoreAttractorComponent);
				this.AddChildDeferred(scoreAttractorComponent);

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
		var delta = GetPhysicsProcessDeltaTime();
		FollowPlayerComponent?.Follow(delta);
		VelocityComponent.MoveAndCollide(this, delta);

		_CheckHealth();
		_CheckSpeed();
	}

	private void EnterStateWalk()
	{
		AnimatedSprite2D.Play("walk");
	}

	private void StateWalk()
	{
		var delta = GetPhysicsProcessDeltaTime();
		FollowPlayerComponent?.Follow(delta);
		VelocityComponent.MoveAndCollide(this, delta);

		_CheckHealth();
		_CheckSpeed();
	}
}
