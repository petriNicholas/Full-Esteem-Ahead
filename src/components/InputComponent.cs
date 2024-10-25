using Godot;

namespace Game.Components;

public partial class InputComponent : Node2D
{

	[Export] private VelocityComponent velocityComponent;

    public override void _Ready()
    {
        velocityComponent = GetParent<CharacterBody2D>().GetNode<VelocityComponent>("VelocityComponent");
	}

    public void UserInputMovement()
	{
		Vector2 moveVector = new Vector2(Input.GetActionStrength("right") - Input.GetActionStrength("left"),
										Input.GetActionStrength("down") - Input.GetActionStrength("up"));

		velocityComponent.SetDirection(moveVector);
	}
/*
	public override void _UnhandledInput(InputEvent @event)
	{
		MovementComponent movementComp = GetParent<MovementComponent>();

		if (@event is InputEventKey eventKey)
		{
			if (eventKey.IsActionReleased("roll") && !movementComp.IsRolling())
			{
				movementComp.Roll();
				@event.Set("handled", true);
			}
		}
	}
*/
}
