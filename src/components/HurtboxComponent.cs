using Godot;

namespace Game.Components;

public partial class HurtboxComponent : Area2D
{
	[Signal]
	public delegate void DamageEventHandler(int amount);

	[Export]
	public HealthComponent HealthComponent {get; set;}

	public void ApplyDamage(int amount)
	{
		if(HealthComponent != null)
		{
			HealthComponent.TakeDamage(amount);
			EmitSignal(nameof(Damage), amount);
		}
	}
}
