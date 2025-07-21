using Godot;
using Game.Weapons;

namespace Game.Components;

public partial class AttackComponent : Node2D
{
    [Export] private InputComponent inputComponent;
    [Export] private PackedScene bulletScene;

    public override void _Ready()
    {
        inputComponent.Attack += Setup;
    }

    public void Setup()
    {
        var _characterNode = GetParent<CharacterBody2D>();

        Vector2 Direction = (GetGlobalMousePosition() - _characterNode.Position).Normalized();

        var bullet = bulletScene.Instantiate<Bullet>();
        bullet.Initialize(Direction, _characterNode.Position);

        GetTree().CurrentScene.AddChild(bullet);
    }
}