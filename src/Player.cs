using Game.Components;
using Godot;

namespace Game;

public partial class Player : CharacterBody2D
{

    [Export] private InputComponent inputComponent;
    [Export] private HealthComponent healthComponent;

    private VelocityComponent velocityComponent;

    private AnimatedSprite2D _animatedSprite;


    private bool _isRolling = false;
    private float _rollingTimer = 0.05f;
    private float _rollSpeedMultiplier = 8.0f;
    private float _rollCooldown = 0f;
    private float _rollCooldownTimer = 0.0f;
    private Vector2 _rollVelocity = Vector2.Zero;
    public bool IsRolling() => _isRolling;


    public override void _Ready()
    {
        velocityComponent = GetNode<VelocityComponent>("VelocityComponent");

        _animatedSprite = GetNode<AnimatedSprite2D>("WalkAnimation");

        _animatedSprite.Play("default");

        inputComponent.isRollingCallback = IsRolling;

        inputComponent.Roll += Roll;
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateSpriteDirection();

        if (_isRolling)
        {
            HandleRoll(delta);
        }
        else
        {
            HandleCooldown(delta);
        }
    }

    private void UpdateSpriteDirection()
    {
        Vector2 direction = inputComponent.GetDirection();

        if (direction.X < 0)
            _animatedSprite.FlipH = true;
        else if (direction.X > 0)
            _animatedSprite.FlipH = false;
    }

    public void Roll()
    {
        if (!IsRolling() && _rollCooldownTimer <= 0) return;

        Vector2 rollDirection = inputComponent.GetDirection();

        if (rollDirection != Vector2.Zero)
        {
            _rollVelocity = rollDirection.Normalized() * _rollSpeedMultiplier * velocityComponent.MaxSpeed;

            velocityComponent.SetDirection(Vector2.Zero);

            _isRolling = true;
            _rollingTimer = 0.05f;
            _rollCooldownTimer = _rollCooldown;
        }
    }

    private void HandleRoll(double delta)
    {
        this.Velocity = _rollVelocity;
        this.MoveAndSlide();

        _rollingTimer -= (float)delta;

        if (_rollingTimer <= 0)
        {
            _isRolling = false;
            _rollingTimer = 0.05f;
            this.Velocity = Vector2.Zero;

            inputComponent.UserInputMovement();
        }
    }

    private void HandleCooldown(double delta)
    {
        if (_rollCooldownTimer > 0)
        {
            _rollCooldownTimer -= (float)delta;
        }
    }

}
