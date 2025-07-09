using Godot;
using Game.Components;
using Game;

public partial class Bullet : Node2D
{
	[Export] public HitboxComponent hitboxComponent;
	[Export] private float _acceleration { get; set; } = 1.0f;
	[Export] private float _maxSpeed { get; set; } = 100.0f;
	[Export] private float _lifetime { get; set; } = 10.0f;

	private float _speed = 0.0f;
	private Vector2 _direction;
	private Vector2 _position;
	private float _timer = 0.0f;

	public void Initialize(Vector2 direction, Vector2 position)
	{
		Position = position;
		_direction = direction.Normalized();
		Rotation = direction.Angle();
		_speed = _maxSpeed;
	}

	public override void _Ready()
	{
		hitboxComponent.AreaEntered += OnCollision;

		var _animatedSprite = GetNode<AnimatedSprite2D>("BulletAnimation");
		_animatedSprite.Play("shot");
	}

	public override void _Process(double delta)
	{
		_speed -= _acceleration * (float) delta;
		// Mathf.Clamp(_speed, 0, _maxSpeed);
		GD.Print(_speed);
		_speed = Mathf.Min(_speed, _maxSpeed);
		Position += _direction * _speed * (float)delta;

		_timer += (float)delta;
		if (_timer >= _lifetime || _speed <= 0)
			QueueFree();
	}

	private void OnCollision(Area2D area)
	{
		if (area.GetParent() is Bullet or Player) return;
		
		if (area is HurtboxComponent hurtbox)
			hurtbox.ApplyDamage(10);

		QueueFree();
	}
}