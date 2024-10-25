using Godot;

namespace Game.Components;

public partial class HurtboxComponent : Area2D
{
	[Signal]
	public delegate void DamageEventHandler(int amount);

	[Export]
	public HealthComponent HealthComponent {get; set;}
	[Export]
	public VisualCuesComponent visualCuesComponent {get; set;}

	public void ApplyDamage(int amount)
	{
		if(HealthComponent != null)
		{
			HealthComponent.TakeDamage(amount);

			VisualCuesComponent visualCueInstance = new VisualCuesComponent();
			GetParent().AddChild(visualCueInstance);
			visualCueInstance.ShowDamage(amount, Position);

			EmitSignal(nameof(Damage), amount);
		}
	}
}
