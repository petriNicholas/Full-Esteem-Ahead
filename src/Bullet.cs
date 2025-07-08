using Godot;
using Game.Components;
using Game;

public partial class Bullet : Node2D
{
	[Export] public HitboxComponent hitboxComponent;
	[Export] private float _acceleration { get; set; } = 10.0f;
	[Export] private float _maxSpeed { get; set; } = 100.0f;
	[Export] private float _lifetime { get; set; } = 3.0f;

	private float _speed = 0.0f;
	private Vector2 _direction;
	private float _timer = 0.0f;

	public void Initialize(Vector2 direction)
	{
		_direction = direction;
		Rotation = direction.Angle();
	}

	public override void _Ready()
	{
		hitboxComponent.AreaEntered += OnCollision;

		var _animatedSprite = GetNode<AnimatedSprite2D>("BulletAnimation");
		_animatedSprite.Play("shot");
	}

	public override void _Process(double delta)
	{
		_speed += (_maxSpeed - _acceleration) * (float)delta;
		_speed = Mathf.Min(_speed, _maxSpeed);
		Position += _direction * _speed * (float)delta;

		_timer += (float)delta;
		if (_timer >= _lifetime)
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
