using Godot;

namespace Game.Enemies;

public partial class EnemyRat : CharacterBody2D
{
	private AnimatedSprite2D animatedSprite;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("RatAnimation");

		animatedSprite.Play("Walk");
	}

	public override void _Process(double delta)
	{
	}
}
