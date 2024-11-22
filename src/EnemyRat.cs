using Game.Components;
using Godot;

namespace Game.Enemies;

public partial class EnemyRat : CharacterBody2D
{
	[Export] private PathfindingComponent pathfindingComponent;
	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("RatAnimation");

		_animatedSprite.Play("Walk");

		Node2D player = GetNode<Node2D>("../Player");
        pathfindingComponent.SetTarget(player);
	}

	public override void _Process(double delta)
	{
	}
}
