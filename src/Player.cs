using Godot;
using System;

public partial class Player : CharacterBody2D
{

	[Export] private MovementComponent movementComponent;

	public override void _PhysicsProcess(double delta)
	{
		movementComponent.Movement(delta);
	}
}
