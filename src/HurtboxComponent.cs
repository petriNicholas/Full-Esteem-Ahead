using Godot;
using System;

public partial class HurtboxComponent : Area2D
{
	[Signal]
	public delegate void DamageEventHandler(float amount);

	[Export]
	public HealthComponent HealthComponent {get; set;}

	public void ApplyDamage(float amount)
	{
		if(HealthComponent != null)
		{
			HealthComponent.Damage(amount);
			EmitSignal(nameof(Damaged), amount);
		}
	}
}
