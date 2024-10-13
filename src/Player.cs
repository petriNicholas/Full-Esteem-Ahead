using Godot;
using System;

public partial class Player : CharacterBody2D
{

	[Export] private MovementComponent movementComponent;
	[Export] private HealthComponent healthComponent;

	public override void _PhysicsProcess(double delta)
	{
		movementComponent.Movement(delta);
	}
}
