using Game.Components;
using Godot;

namespace Game.Enemies;

public partial class EnemyRat : CharacterBody2D
{
	[Export] public float AttackRange = 20f;
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

		bbSimpleStateMachine.TransitionTo("Walk"); // to fix: zmiana tego na InitialState powoduje błędy
	}

	// ====== Stan "Walk" ======
	public void Walk_enter()
	{
		GD.Print("Entering Walk state.");
		pathfindingComponent.velocityComponent.SetMaxSpeed(100);
	}

	public void Walk_process(float delta)
	{
		GD.Print("Walking");
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
	public void Walk_exit()
	{
		GD.Print("Exiting Walk state.");
	}

	// ====== Stan "Attack" ======
	public void Attack_enter()
	{
		GD.Print("Entering Attack state.");
		pathfindingComponent.velocityComponent.SetMaxSpeed(0);
	}

	public void Attack_process(float delta)
	{
		GD.Print("attacking");
		if (Position.DistanceTo(_player.Position) > AttackRange)
		{
			GD.Print("Player is out of range. Returning to Walk.");
			bbSimpleStateMachine.TransitionTo("Walk");
		}
	}

	public void Attack_exit()
	{
		GD.Print("Exiting Attack state.");
	}

	// ====== Stan "Sprint" ======
	public void Sprint_enter()
	{
		GD.Print("Entering Sprint state.");
		pathfindingComponent.velocityComponent.SetMaxSpeed(300);
	}

	public void Sprint_process(float delta)
	{
		GD.Print("Sprinting");
		if (_player != null && Position.DistanceTo(_player.Position) < 400)
		{
			bbSimpleStateMachine.TransitionTo("Walk");
			return;
		}
	}

	public void Sprint_exit()
	{
		GD.Print("Exiting Sprint state.");
	}
}
