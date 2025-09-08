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

        hitboxComponent.Hit += OnHit;
    }

    public override void _Process(double delta)
    {
        _player = GetNode<Node2D>("../Player");
        pathfindingComponent.SetTarget(_player);
    }

    // ====== Stan "Walk" ======
    public void Walk_enter()
    {
        pathfindingComponent.velocityComponent.SetSpeedModifier(1.0f);
    }

    public void Walk_physics_process(float delta)
    {
        UpdateSpriteDirection();

        if (_player != null && Position.DistanceTo(_player.Position) <= AttackRange)
        {
            bbSimpleStateMachine.TransitionTo("Attack");
            return;
        }
    }

    // ====== Stan "Attack" ======
    public async void Attack_enter()
    {
        pathfindingComponent.PauseNagivation(true);
        pathfindingComponent.velocityComponent.SetSpeedModifier(0f);

        _animatedSprite.Play("Jump");

        await ToSignal(GetTree().CreateTimer(1.0), "timeout");

        UpdateSpriteDirection();
        Vector2 jumpDirection = (_player.GlobalPosition - GlobalPosition).Normalized();
        pathfindingComponent.velocityComponent.SetDirection(jumpDirection);
        pathfindingComponent.velocityComponent.SetSpeedModifier(2.0f);

        await ToSignal(GetTree().CreateTimer(0.5), "timeout");

        pathfindingComponent.velocityComponent.SetSpeedModifier(0f);
        _animatedSprite.Play("Idle");
        await ToSignal(GetTree().CreateTimer(0.5), "timeout");

        pathfindingComponent.PauseNagivation(false);
        bbSimpleStateMachine.TransitionTo("Walk");
    }
    // ===========================

    private void OnHit(HurtboxComponent hurtbox, int amount)
    {
        var target = hurtbox.GetOwner();

        if (target is Player)
            hurtbox.ApplyDamage(amount);
    }

    private void UpdateSpriteDirection()
    {
        _animatedSprite.FlipH = _player.GlobalPosition.X < GlobalPosition.X;
    }
}
