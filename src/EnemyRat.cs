using Game.Components;
using Godot;

namespace Game.Enemies;

public partial class EnemyRat : CharacterBody2D
{
	[Export] public float AttackRange = 100f;
	[Export] private PathfindingComponent pathfindingComponent;
	[Export] private BbSimpleStateMachine bbSimpleStateMachine;
	private Node2D _player;
	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("RatAnimation");
		_animatedSprite.Play("Walk");

		_player = GetNode<Node2D>("../Player");
		pathfindingComponent.SetTarget(_player);

		bbSimpleStateMachine.TransitionTo("Walk");
	}

	// ====== Stan "Walk" ======
	public void Walk_enter()
	{
		pathfindingComponent.velocityComponent.SetMaxSpeed(300);
	}

	public void Walk_process(float delta)
	{
		if (_player != null && Position.DistanceTo(_player.Position) >= 400)
		{
			bbSimpleStateMachine.TransitionTo("Sprint");
			return;
		}
		if (_player != null && Position.DistanceTo(_player.Position) <= AttackRange)
		{
			bbSimpleStateMachine.TransitionTo("Attack");
			return;
		}
	}

	// ====== Stan "Attack" ======
	public void Attack_enter()
	{
		pathfindingComponent.velocityComponent.SetMaxSpeed(0);
	}

	public void Attack_process(float delta)
	{
		if (Position.DistanceTo(_player.Position) > AttackRange)
		{
			bbSimpleStateMachine.TransitionTo("Walk");
		}
	}
}
