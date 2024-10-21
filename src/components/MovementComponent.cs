using Godot;

namespace Game.Components;

public partial class MovementComponent : Node2D
{

	[Export(PropertyHint.Range, "0, 300")] private float _speed = 3.0f;
	private Vector2 moveVector = Vector2.Zero;
	[Export] private InputComponent _InputComponent;
	

	private bool _isRolling = false;
	private float _rollingTimer = 0.5f;

    public void Movement(double delta)
	{
		moveVector = _InputComponent.UserInputMovement();

		CharacterBody2D parentNode = GetParent<CharacterBody2D>();

		Vector2 velocity = Vector2.Zero;

		HandleRoll(delta);

		velocity = moveVector.Normalized() * _speed;
		parentNode.Velocity = velocity;
		parentNode.MoveAndSlide();
	}

	public void Roll()
	{
		CharacterBody2D parentNode = GetParent<CharacterBody2D>();
		Vector2 moveVector = _InputComponent.UserInputMovement();

		Vector2 velocity = moveVector.Normalized() * _speed * 10;

		_isRolling = true;
		_rollingTimer = 0.5f;
		parentNode.Velocity = velocity;

		parentNode.MoveAndSlide();
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

	public bool IsRolling()
	{
		return _isRolling;
	}
}
