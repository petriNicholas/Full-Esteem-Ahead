using Godot;

namespace Game.Components;

public partial class VelocityComponent : Node2D
{

	private static readonly Vector2 DecelerationTargetVelocity = Vector2.Zero;

	[Export(PropertyHint.Range, "0, 300")] public float MaxSpeed {get; private set;} = 3.0f;

	[Export(PropertyHint.Range, "0, 1")] public float AccelerationCoefficient {get; set;} = 1.0f;
	[Export(PropertyHint.Range, "0, 1")] public float DecelerationCoefficient {get; set;} = 1.0f;

	public Vector2 Direction {get; private set;} = Vector2.Zero;
	

	//private bool _isRolling = false;
	//private float _rollingTimer = 0.5f;

	private CharacterBody2D _charaterNode;

    public override void _Ready()
    {
        _charaterNode = GetParent<CharacterBody2D>();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Direction != Vector2.Zero)
		{
			Accelerate(delta);
		}
		else if(_charaterNode.Velocity != DecelerationTargetVelocity)
		{
			Decelerate(delta);
		}

		_charaterNode.MoveAndSlide();
    }

    public void Accelerate(double delta)
	{
		//var accererationRate = MaxSpeed * AccelerationCoefficient * (float)delta;

		var targetSpeed = Direction.Normalized() * MaxSpeed;

		_charaterNode.Velocity = targetSpeed;
	}
	
	public void Decelerate(double delta)
	{
		//var decererationRate = MaxSpeed * DecelerationCoefficient * (float)delta;

		_charaterNode.Velocity = DecelerationTargetVelocity;
	}

	public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    /*
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
    */

}
