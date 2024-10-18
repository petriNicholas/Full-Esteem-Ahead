using Godot;
using System;

public partial class HitboxComponent : Area2D
{
	[Signal]
	public delegate void HitEventHandler(HurtboxComponent hurtbox, float amount);

	[Export]
	public float DamageAmount{get; private set;} = 1.0f;

	public void OnHurtboxEntered(HurtboxComponent hurtbox)
	{
		hurtbox.ApplyDamage(DamageAmount);
		EmitSignal(nameof(Hit), hurtbox, DamageAmount);
	}
}
