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

		if (bbSimpleStateMachine == null)
{
    GD.PrintErr("BbSimpleStateMachine not assigned in EnemyRat.");
    return;
}
	}

	// ====== Stan "Walk" ======
	public void Walk_enter()
{
    GD.Print("Entering Walk state.");
}
	public void Walk_physics_process(float delta)
    {
		// niech przeciwnik otrzyma velocity, żeby mógł iśc w stronę gracza

		if (_player != null && Position.DistanceTo(_player.Position) <= AttackRange)
        {
            bbSimpleStateMachine.TransitionTo("Attack"); // Przejście do stanu ataku
			return;
        }

		Vector2 direction = pathfindingComponent.GetDirection();
        pathfindingComponent.velocityComponent.SetDirection(direction);
	}
	public void Walk_exit()
{
    GD.Print("Exiting Walk state.");
}

	// ====== Stan "Attack" ======
	public void Attack_enter()
{
    GD.Print("Entering Attack state.");
}
	public void Attack_physics_process(float delta)
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
}
