using Godot;

public partial class HealthComponent : Node
{
	private int maxHealth = 100;
	private int currentHealth;
	private bool isDead;

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
		get => maxHealth;
		private set
		{
			maxHealth = value;
			if(CurrentHealth > maxHealth)
			{
				CurrentHealth = maxHealth;
			}
		}
	}

	public int CurrentHealth
	{
		get => currentHealth;
		private set
		{
			int oldHealth = currentHealth;
			currentHealth = value;

			EmitSignal(nameof(HealthChanged), currentHealth);

			if(currentHealth <= 0 && !isDead)
			{
				currentHealth = 0;
				isDead = true;
				EmitSignal(nameof(Died));
				OnDeath();
			}
			else if(currentHealth > maxHealth)
			{
				currentHealth = maxHealth;
			}
			else if (isDead && currentHealth > 0)
			{
				isDead = false;

				EmitSignal(nameof(Revived));
			}
		}
	}

	public bool IsDead => isDead;

	public override void _Ready()
    {
        CurrentHealth = MaxHealth;
        isDead = false;
    }

	public void TakeDamage(int damage)
	{
		if(isDead) return;

		int oldHealth = CurrentHealth;
		CurrentHealth -= damage;

		EmitSignal(nameof(Damaged), oldHealth - CurrentHealth);
	}
	
	public void Heal(int amount,  bool canRevive = false)
	{
		if ((isDead && !canRevive) || amount < 0) return;

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
