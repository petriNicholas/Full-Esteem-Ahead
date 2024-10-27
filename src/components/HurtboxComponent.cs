using Godot;

namespace Game.Components;

public partial class HurtboxComponent : Area2D
{
	[Signal]
	public delegate void DamageEventHandler(int amount);

	[Export]
	public HealthComponent HealthComponent {get; private set;}
	[Export]
	public PackedScene visualCuesComponent {get; set;}

	public void ApplyDamage(int amount)
	{
		if(HealthComponent != null)
		{
			HealthComponent.TakeDamage(amount);

			VisualCuesComponent visualCueInstance = visualCuesComponent.Instantiate<VisualCuesComponent>();
			// visualCueInstance._label.Text = amount.ToString();
			visualCueInstance.ShowDamage(amount, Position);

			GetParent().AddChild(visualCueInstance);

			EmitSignal(nameof(Damage), amount);
		}
	}
}
