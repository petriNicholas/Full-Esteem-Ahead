using Game.Enemies;
using Godot;

namespace Game.Components;

public partial class ShootingComponent : Node2D
{
	[Export] public float StartingSpeed { get; set; }
	[Export] public Vector2 Direction { get; set; } = new Vector2(0, 1);
	[Export] public float Radius { get; set; } = 9.0f;

	private float _initColRadius;
	private float _acceleration = 0.0f;
	private float _speed;
	private float _maxSpeed;
	private float _lifetime;
	private float _checkBoundaryTime;
	private float _spentTime;

	private float _targetScale = 1.0f;
	private bool _grazed = false;
	private bool _died = false;

	private float _angularSpeed = 0.0f;
	private float _angularStray = 0.0f;
	private float _maxAngularStray = 0.0f;

	public void Setup()
	{
		Bullet bulletType = new Bullet();
		
		if (StartingSpeed == 0)
			StartingSpeed = bulletType.Speed;
		_speed = StartingSpeed;
		_maxSpeed = bulletType.MaxSpeed;
		_angularSpeed = Mathf.DegToRad(bulletType.AngularSpeed);
		_maxAngularStray = Mathf.DegToRad(bulletType.MaxAngularStray);
		_acceleration = bulletType.Acceleration;
		_lifetime = bulletType.Lifetime;

		Direction = (GetGlobalMousePosition() - Position).Normalized();

		this.AddChild(bulletType);
	}

	public override void _Ready()
	{
		_spentTime = 0.0f;
		Direction = Direction.Normalized();
	}

	public void _PhysicsProcess(float delta)
	{
		MoveBullet(delta);
		CheckCollisions();
	}

	private void MoveBullet(float delta)
	{
		_angularStray += _angularSpeed * delta;
		if (_maxAngularStray == 0 || Mathf.Abs(_angularStray) < Mathf.Abs(_maxAngularStray))
		{
			Direction = Direction.Rotated(_angularSpeed * delta);
		}

		Rotation = -Direction.AngleTo(Vector2.Up);

		Position += Direction * _speed * delta;
		if (_maxSpeed == 0 || Mathf.Abs(_speed) < Mathf.Abs(_maxSpeed))
		{
			_speed += _acceleration * delta;
		}

		_spentTime += delta;
	}

	private void CheckCollisions()
	{
		float distSquaredToPlayer = GetGlobalMousePosition().DistanceSquaredTo(GlobalPosition);

		if (_spentTime > _lifetime)
		{
			Die();
		}
	}

	private void Die()
	{
		if (_died) return;
		_died = true;
		GetParent().RemoveChild(this);
		QueueFree();
	}

	private void OnCollision(Area2D area)
	{
		if (area is HurtboxComponent hurtbox)
		{
			hurtbox.ApplyDamage(10);
		}
	}
}