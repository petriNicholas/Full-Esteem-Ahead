using Godot;

public partial class InputComponent : Node2D
{
	public Vector2 UserInputMovement()
	{
		Vector2 moveVector = new Vector2(Input.GetActionStrength("right") - Input.GetActionStrength("left"),
										Input.GetActionStrength("down") - Input.GetActionStrength("up"));

		return moveVector;
	}
}
