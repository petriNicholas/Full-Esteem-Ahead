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
	bbSimpleStateMachine.StateOwner = this; // Ustaw EnemyRat jako właściciela
    bbSimpleStateMachine.MethodPrefix = "";

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
	pathfindingComponent.SetTarget(_player);
}
	public void Walk_physics_process(float delta)
    {
		GD.Print("Walking");
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
	pathfindingComponent.SetTarget(null);
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
