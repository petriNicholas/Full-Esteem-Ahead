using System.Threading.Tasks;
using Godot;
using Microsoft.VisualBasic;

namespace Game.Components;

public partial class AttackComponent : Node2D
{
	[Export] private InputComponent inputComponent;
	[Export] public Vector2 Direction { get; set; } = new Vector2(0, 1);
	[Export] private PackedScene bulletScene;

	public void Setup()
	{
		Direction = (GetGlobalMousePosition() - Position).Normalized();

		var bullet = bulletScene.Instantiate<Bullet>();

		bullet.Initialize(Direction);

		var parent = GetParent();

		parent.AddChild(bullet);

	}

	public override void _Ready()
	{
		Direction = Direction.Normalized();

		inputComponent.Attack += Setup;
	}
}