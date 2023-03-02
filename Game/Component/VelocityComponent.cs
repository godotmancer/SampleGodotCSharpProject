using Game.Autoload;

namespace Game.Component;

public partial class VelocityComponent : BaseComponent
{
	[Signal]
	public delegate void CollidedEventHandler(KinematicCollision2D collision2D);

	[Export]
	public CollisionShape2D CollisionShape2D;

	[Export]
	public PhysicsBody2D ContinuousProcess;

	[Export]
	public Vector2 Gravity = new(0f, 0f);

	[Export]
	public Vector2 Velocity = Vector2.Zero;

	[Export]
	public bool JustMove;

	public bool Falling { get; private set; }


	public float Speed { get; private set; }

	private Vector2 _lastPosition = Vector2.Zero;

	public override void _EnterTree()
	{
		this.WireNodes();
	}

	public override void _Ready()
	{
		SetPhysicsProcess(false);
	}

	private void _UpdateSpeed()
	{
		Speed = (_lastPosition - GlobalPosition).LengthSquared();
		_lastPosition = GlobalPosition;
	}

	private KinematicCollision2D _CalculateSpeed(Func<KinematicCollision2D> action)
	{
		var result = action.Invoke();
		_UpdateSpeed();
		return result;
	}

	private bool _CalculateSpeed(Func<bool> action)
	{
		var result = action.Invoke();
		_UpdateSpeed();
		return result;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Enabled) return;

		MoveAndCollide(ContinuousProcess, delta);
		Falling = Gravity.LengthSquared() > 0.0f;
	}

	public void DisableCollisionCheck(bool flag)
	{
		CollisionShape2D?.CallDeferred(CollisionShape2D.MethodName.SetDisabled, flag);
	}

	public KinematicCollision2D MoveAndCollide(PhysicsBody2D node, double delta)
	{
		if (!Enabled) return null;

		Velocity += Gravity;

		var collision2D = _CalculateSpeed( () =>
		{
			if (JustMove)
			{
				node.GlobalPosition += Velocity * (float)delta;
				return null;
			}
			return node.MoveAndCollide(Velocity * (float)delta);
		});
		if (collision2D == null) return null;

		_EmitCollision(collision2D);

		return collision2D;
	}

	public void MoveAndSlide(CharacterBody2D node)
	{
		if (!Enabled) return;

		Velocity += Gravity;
		node.Velocity = Velocity;
		var collided = _CalculateSpeed( () =>
		{
			if (JustMove)
			{
				node.GlobalPosition += Velocity;
				return false;
			}
			return node.MoveAndSlide();
		});

		if (!collided) return;

		var collision2D = node.GetLastSlideCollision();
		_EmitCollision(collision2D);
	}

	private void _EmitCollision(KinematicCollision2D collision2D)
	{
		EmitSignal(SignalName.Collided, collision2D);
		GameEvents.EmitCollision(collision2D);
	}

	public void EnablePhysics(bool flag)
	{
		SetPhysicsProcess(flag);
		if (flag == false)
		{
			Falling = false;
		}
	}

	public void ApplyGravity(Node2D node, Vector2 gravity)
	{
		Velocity = Vector2.Zero;
		Gravity = gravity;
		ContinuousProcess = node as PhysicsBody2D;
		SetPhysicsProcess(true);
	}
}
