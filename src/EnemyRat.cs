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

		bbSimpleStateMachine.MethodPrefix = "";
		bbSimpleStateMachine.TransitionTo("Walk");
	}

	// ====== Stan "Walk" ======
	public void Walk_process(float delta)
    {
		// niech przeciwnik otrzyma velocity, żeby mógł iśc w stronę gracza

		if (_player != null && Position.DistanceTo(_player.Position) <= AttackRange)
        {
            bbSimpleStateMachine.TransitionTo("Attack"); // Przejście do stanu ataku
        }

		Vector2 direction = pathfindingComponent.GetDirection();
        pathfindingComponent.velocityComponent.SetDirection(direction);
	}

	// ====== Stan "Attack" ======
	public void Attack_process(float delta)
    {
		GD.Print("attacking");
        if (Position.DistanceTo(_player.Position) > AttackRange)
        {
            GD.Print("Player is out of range. Returning to idle.");
            bbSimpleStateMachine.TransitionTo("idle");
        }
    }
}
