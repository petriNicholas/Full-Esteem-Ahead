using Game.Components;
using Godot;

namespace Game;

public partial class Player : CharacterBody2D
{

    [Export] private InputComponent inputComponent;
    [Export] private HealthComponent healthComponent;

    private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("WalkAnimation");
        _animatedSprite.Play("default");
    }

    public override void _PhysicsProcess(double delta)
    {
        inputComponent.UserInputMovement();
        UpdateSpriteDirection();
    }

    private void UpdateSpriteDirection()
    {
        Vector2 direction = inputComponent.GetDirection();

        if (direction.X < 0)
            _animatedSprite.FlipH = true;
        else if (direction.X > 0)
            _animatedSprite.FlipH = false;
    }
}
