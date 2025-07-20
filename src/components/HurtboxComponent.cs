using Godot;

namespace Game.Components;

public partial class HurtboxComponent : Area2D
{
    [Signal] public delegate void DamageEventHandler(int amount);

    [Export] public HealthComponent HealthComponent { get; private set; }
    [Export] public PackedScene VisualCuesComponent { get; private set; }

    public void ApplyDamage(int amount)
    {
        HealthComponent.TakeDamage(amount);

        VisualCuesComponent visualCueInstance = VisualCuesComponent.Instantiate<VisualCuesComponent>();
        visualCueInstance.ShowDamage(amount, Position);
        GetParent().AddChild(visualCueInstance);

        EmitSignal(nameof(Damage), amount);
    }
}
