using Godot;

namespace Game.Enemies;

public partial class EnemyRat : CharacterBody2D
{
	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("RatAnimation");

		_animatedSprite.Play("Walk");
	}

	public override void _Process(double delta)
	{
	}
}
