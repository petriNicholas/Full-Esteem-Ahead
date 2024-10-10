using Godot;

public partial class MovementComponent : Node2D
{

	[Export(PropertyHint.Range, "0, 300")] private float _speed = 3.0f;
	private Vector2 moveVector = Vector2.Zero;
	
	private bool _isRolling = false;
	private float rollingTimer = 0.5f;

	public void Movement(double delta)
	{
		moveVector = Input.GetVector("left", "right", "up", "down");

		CharacterBody2D player = GetParent<CharacterBody2D>();
		Vector2 velocity = Vector2.Zero;

		if(_isRolling)
		{
			rollingTimer -= (float)delta;
			if (rollingTimer == 0) 
			{
				_isRolling = false;
				rollingTimer = 0.5f;
			}
		}
		else
		{
			velocity = moveVector.Normalized() * _speed;
			player.Velocity = velocity;
			player.MoveAndSlide();
		}
		
		
	}

	public void Roll()
	{
		CharacterBody2D player = GetParent<CharacterBody2D>();
		Vector2 rollV = new Vector2(10, 10);

		rollV.Rotated( player.Position.AngleTo(GetGlobalMousePosition()) );

		Vector2 velocity = rollV.Normalized() * _speed;
		_isRolling = true;

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

				@event.Set("handled", true);
			}
		}
	}

}
