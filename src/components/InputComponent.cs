using Godot;

namespace Game.Components;

public partial class InputComponent : Node2D
{
    [Signal]
    public delegate void AttackEventHandler();

    [Export] private VelocityComponent velocityComponent;

    private bool _isRolling = false;
    private float _rollingTimer = 0.05f;
    private float _rollSpeedMultiplier = 8.0f;
    private float _rollCooldown = 0f;
    private float _rollCooldownTimer = 0.0f;
    private Vector2 _rollVelocity = Vector2.Zero;

    public override void _PhysicsProcess(double delta)
    {
        if (_isRolling)
        {
            HandleRoll(delta);
        }
        else
        {
            UserInputMovement();
            HandleCooldown(delta);
        }
    }

    public void UserInputMovement()
    {
        if (_isRolling) return;

        Vector2 moveVector = new Vector2(Input.GetActionStrength("right") - Input.GetActionStrength("left"),
                                        Input.GetActionStrength("down") - Input.GetActionStrength("up"));

        velocityComponent.SetDirection(moveVector);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.IsActionPressed("roll") && !IsRolling() && _rollCooldownTimer <= 0)
            {
                Roll();
                @event.Set("handled", true);
            }
        }

        if (@event is InputEventMouse eventMouse)
        {
            if (eventMouse.IsActionPressed("shoot"))
            {
                EmitSignal(SignalName.Attack);
                @event.Set("handled", true);
            }
        }
    }
    public void Roll()
    {
        Vector2 rollDirection = velocityComponent.Direction;

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
        CharacterBody2D character = GetParent<CharacterBody2D>();
        character.Velocity = _rollVelocity;
        character.MoveAndSlide();

        _rollingTimer -= (float)delta;

        if (_rollingTimer <= 0)
        {
            _isRolling = false;
            _rollingTimer = 0.05f;
            character.Velocity = Vector2.Zero;
            UserInputMovement();
        }
    }

    private void HandleCooldown(double delta)
    {
        if (_rollCooldownTimer > 0)
        {
            _rollCooldownTimer -= (float)delta;
        }
    }

    private bool IsRolling() => _isRolling;

    public Vector2 GetDirection() => velocityComponent.Direction;
}
