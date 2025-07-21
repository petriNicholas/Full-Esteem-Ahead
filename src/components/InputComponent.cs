using System;
using Godot;

namespace Game.Components;

public partial class InputComponent : Node2D
{
    [Signal]
    public delegate void AttackEventHandler();
    [Signal]
    public delegate void RollEventHandler();

    [Export] private VelocityComponent velocityComponent;

    public Func<bool>? isRollingCallback;

    public Vector2 GetDirection() => velocityComponent.Direction;

    public override void _PhysicsProcess(double delta)
    {
        UserInputMovement();
    }

    public void UserInputMovement()
    {
        if (isRollingCallback != null && isRollingCallback()) return;

        Vector2 moveVector = new Vector2(Input.GetActionStrength("right") - Input.GetActionStrength("left"),
                                        Input.GetActionStrength("down") - Input.GetActionStrength("up"));

        velocityComponent.SetDirection(moveVector);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.IsActionPressed("roll"))
            {
                EmitSignal(SignalName.Roll);
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

}
