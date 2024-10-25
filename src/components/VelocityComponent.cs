using Godot;

namespace Game.Components;

public partial class VelocityComponent : Node2D
{
	[Export(PropertyHint.Range, "0, 300, or_greater, hide_slider")] public float MaxSpeed {get; private set;} = 100.0f;
	[Export(PropertyHint.Range, "0, 1")] public float AccelerationCoefficient {get; private set;} = 1.0f;
	[Export(PropertyHint.Range, "0, 1")] public float DecelerationCoefficient {get; private set;} = 1.0f;

	public Vector2 Direction {get; private set;} = Vector2.Zero;
	
	private CharacterBody2D _characterNode;

    public override void _Ready()
    {
        _characterNode = GetParent<CharacterBody2D>();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Direction != Vector2.Zero)
		{
			Accelerate(delta);
		}
		else if(_characterNode.Velocity != Vector2.Zero)
		{
			Decelerate(delta);
		}

		_characterNode.MoveAndSlide();
    }

    public void Accelerate(double delta)
	{
		//float accelerationRate = MaxSpeed * AccelerationCoefficient * (float)delta;

		Vector2 targetSpeed = Direction.Normalized() * MaxSpeed;

		//_characterNode.Velocity = _characterNode.Velocity.MoveToward(targetSpeed, accelerationRate);

        _characterNode.Velocity = targetSpeed;
	}
	
	public void Decelerate(double delta)
	{
		//float decelerationRate = MaxSpeed * DecelerationCoefficient * (float)delta;

		//_characterNode.Velocity = _characterNode.Velocity.MoveToward(Vector2.Zero, decelerationRate);

        _characterNode.Velocity = Vector2.Zero * (float)delta;
	}

	public void SetDirection(Vector2 direction) => Direction = direction;

    public Vector2 GetDirection() => Direction;
}
