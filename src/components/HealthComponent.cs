using Godot;

public partial class HealthComponent : Node
{
	private int maxHealth = 100;
	private int currentHealth;
	private bool isDead;

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
			currentHealth = value;
			if(currentHealth <= 0 && !isDead)
			{
				currentHealth = 0;
				isDead = true;
				OnDeath();
			}
			else if(currentHealth > maxHealth)
			{
				currentHealth = maxHealth;
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

		CurrentHealth -= damage;
	}
	
	private void OnDeath()
	{
		GetParent().Free();
	}
}
