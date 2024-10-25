using Godot;

namespace Game;

public partial class Player : CharacterBody2D
{

	[Export] private Components.InputComponent inputComponent;
	[Export] private Components.HealthComponent healthComponent;

	private AnimatedSprite2D animatedSprite;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("WalkAnimation");

		animatedSprite.Play("default");
	}

	public override void _PhysicsProcess(double delta)
	{
		inputComponent.UserInputMovement();
	}
}
