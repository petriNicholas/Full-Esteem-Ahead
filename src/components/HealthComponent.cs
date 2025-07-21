using Godot;

namespace Game.Components;

public partial class HealthComponent : Node
{
    [Signal] public delegate void HealthChangedEventHandler(int health);
    [Signal] public delegate void HealedEventHandler(int amount);
    [Signal] public delegate void DamagedEventHandler(int amount);
    [Signal] public delegate void DiedEventHandler();

    private int _maxHealth = 100;
    private int _currentHealth;
    private bool _isDead = false;
    public bool IsDead => _isDead;

    [Export]
    public int MaxHealth
    {
        get => _maxHealth;
        private set
        {
            int diff = value - _maxHealth;
            _maxHealth = value;

            if (diff > 0)
            {
                CurrentHealth += diff;
            }
            if (CurrentHealth > _maxHealth)
            {
                CurrentHealth = _maxHealth;
            }
        }
    }

    public int CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = value;
            EmitSignal(nameof(HealthChanged), _currentHealth);

            if (_currentHealth <= 0 && !IsDead)
            {
                _currentHealth = 0;
                _isDead = true;
                EmitSignal(nameof(Died));
                OnDeath();
            }
            else if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        }
    }

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;
        CurrentHealth -= damage;
        EmitSignal(nameof(Damaged), damage);
    }

    public void Heal(int amount)
    {
        if (_isDead || amount < 0) return;
        CurrentHealth += amount;
        EmitSignal(nameof(Healed), amount);
    }

    public void HealFully()
    {
        Heal(MaxHealth);
    }

    private void OnDeath()
    {
        GetParent().Free();
    }

    public bool IsMaxed()
    {
        return CurrentHealth >= MaxHealth;
    }
}
