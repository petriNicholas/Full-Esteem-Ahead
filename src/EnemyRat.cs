using Game.Components;
using Game.Templates;
using Godot;

namespace Game.Enemies;

public partial class EnemyRat : Templates.Enemy
{
	
	private Node2D _player;

	public override void _Ready()
	{
		animatedSprite.Play("Walk");

		_player = GetNode<Node2D>("../Player");
		pathfindingComponent.SetTarget(_player);

		bbSimpleStateMachine.TransitionTo("Walk");
	}

	// ====== Stan "Walk" ======
	public void Walk_enter()
	{
		pathfindingComponent.velocityComponent.SetSpeedModifier(1.0f);
	}

	public void Walk_physics_process(float delta)
	{
		if (_player != null && Position.DistanceTo(_player.Position) <= AttackRange)
		{
			bbSimpleStateMachine.TransitionTo("Attack");
			return;
		}
	}

	// ====== Stan "Attack" ======
	public void Attack_enter()
	{
		pathfindingComponent.velocityComponent.SetSpeedModifier(0f);
	}

	public void Attack_physics_process(float delta)
	{
		if (Position.DistanceTo(_player.Position) > AttackRange)
		{
			bbSimpleStateMachine.TransitionTo("Walk");
		}
	}
}
