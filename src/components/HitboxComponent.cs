using Godot;
using System;

public partial class HitboxComponent : Area2D
{
	[Signal]
	public delegate void HitEventHandler(HurtboxComponent hurtbox, int amount);

	[Export]
	public int DamageAmount{get; private set;} = 1;

 	public override void _Ready()
    {
    	this.AreaEntered += OnHurtboxEntered;
    }

	public void OnHurtboxEntered(Area2D area)
	{
		if(area is HurtboxComponent hurtbox)
		{
			GD.Print("Hurtbox entered: dealing damage.");
			hurtbox.ApplyDamage(DamageAmount);
			EmitSignal(nameof(Hit), hurtbox, DamageAmount);
		}
	}
}
