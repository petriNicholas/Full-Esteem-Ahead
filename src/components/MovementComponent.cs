using Godot;

public partial class MovementComponent : Node2D
{

	[Export(PropertyHint.Range, "0, 300")] private float _speed = 3.0f;
	private Vector2 moveVector = Vector2.Zero;
	
	private bool _isRolling = false;
	private float _rollingTimer = 0.5f;

	public void Movement(double delta)
	{
		moveVector = Input.GetVector("left", "right", "up", "down");

		CharacterBody2D player = GetParent<CharacterBody2D>();
		Vector2 velocity = Vector2.Zero;

		HandleRoll(delta);

		velocity = moveVector.Normalized() * _speed;
		player.Velocity = velocity;
		player.MoveAndSlide();
	}

	public void Roll()
	{
		CharacterBody2D player = GetParent<CharacterBody2D>();
		Vector2 moveVector = Input.GetVector("left", "right", "up", "down");

		Vector2 velocity = moveVector.Normalized() * _speed * 10;

		_isRolling = true;
		_rollingTimer = 0.5f;
		player.Velocity = velocity;

		player.MoveAndSlide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.IsActionReleased("roll") && !_isRolling)
			{
				Roll();
				GD.Print("Roll");
				@event.Set("handled", true);
			}
		}
	}

	private void HandleRoll(double delta)
	{
		if (_isRolling)
		{
			_rollingTimer -= (float)delta;

			if (_rollingTimer <= 0)
			{
				_isRolling = false;
				_rollingTimer = 0.5f;
			}
		}
	}
}
