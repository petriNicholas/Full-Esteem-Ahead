using Godot;

namespace Game.Components;

public partial class HealthComponent : Node
{
	private int _maxHealth = 100;
	private int _currentHealth;
	private bool _isDead = false;

	[Signal]
	public delegate void HealthChangedEventHandler(int health);

	[Signal]
	public delegate void HealedEventHandler(int amount);

	[Signal]
	public delegate void HealedFullyEventHandler();

	[Signal]
	public delegate void DamagedEventHandler(int amount);

	[Signal]
	public delegate void DiedEventHandler();

	[Signal]
	public delegate void RevivedEventHandler();
	
	[Export]
	public int MaxHealth
	{
		get => _maxHealth;
		private set
		{
			_maxHealth = value;
			if(CurrentHealth > _maxHealth)
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
			int oldHealth = _currentHealth;
			_currentHealth = value;

			EmitSignal(nameof(HealthChanged), _currentHealth);

			if(_currentHealth <= 0 && !_isDead)
			{
				_currentHealth = 0;
				_isDead = true;
				EmitSignal(nameof(Died));
				OnDeath();
			}
			else if(_currentHealth > _maxHealth)
			{
				_currentHealth = _maxHealth;
			}
			else if (_isDead && _currentHealth > 0)
			{
				_isDead = false;

				EmitSignal(nameof(Revived));
			}
		}
	}

	public bool IsDead => _isDead;

	public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }

	public void TakeDamage(int damage)
	{
		if(_isDead) return;

		int oldHealth = CurrentHealth;
		CurrentHealth -= damage;

		EmitSignal(nameof(Damaged), oldHealth - CurrentHealth);
	}
	
	public void Heal(int amount,  bool canRevive = false)
	{
		if ((_isDead && !canRevive) || amount < 0) return;

		int oldHealth = CurrentHealth;
		CurrentHealth += amount;

		EmitSignal(nameof(Healed), CurrentHealth - oldHealth);

		if (CurrentHealth == MaxHealth)
		{
			EmitSignal(nameof(HealedFully));
		}
	}

	public void HealFully()
	{
		Heal(MaxHealth);
	}

	private void OnDeath()
	{
		GetParent().Free();
	}

	public bool IsAlive()
	{
		return CurrentHealth > 0;
	}

	public bool IsMaxed()
	{
		return CurrentHealth >= MaxHealth;
	}
}
