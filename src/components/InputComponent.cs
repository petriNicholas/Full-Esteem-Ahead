using Godot;

namespace Game.Components;

public partial class InputComponent : Node2D
{
	public Vector2 UserInputMovement()
	{
		Vector2 moveVector = new Vector2(Input.GetActionStrength("right") - Input.GetActionStrength("left"),
										Input.GetActionStrength("down") - Input.GetActionStrength("up"));

		return moveVector;
	}

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
}
