using Game.Components;
using Godot;

namespace Game.Enemies;

public partial class EnemyRat : CharacterBody2D
{
    [Export] public HitboxComponent hitboxComponent;
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

        hitboxComponent.Hit += OnHit;
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

    private void OnHit(HurtboxComponent hurtbox, int amount)
	{
		var target = hurtbox.GetOwner();

		if (target is Player)
            hurtbox.ApplyDamage(amount);
	}
}
