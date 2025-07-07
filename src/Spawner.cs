using System;
using Godot;

public partial class Spawner : Node2D
{
	[Signal] public delegate void EnemyCounterIncreaseEventHandler();
	[Export] public PackedScene _enemyScene;

	public override void _Ready()
	{
		SpawnEnemies();
	}

	public void SpawnEnemies()
	{
		Random random = new Random();

		Vector2 randomOffset = new Vector2(
			((float)random.NextDouble() * 10) - ((float)random.NextDouble() * 10),
			((float)random.NextDouble() * 10) - ((float)random.NextDouble() * 10)
		);
		
		var parent = GetParent();
		var enemy = _enemyScene.Instantiate<CharacterBody2D>();

		enemy.Position = randomOffset;

		// parent.AddChild(enemy); TODO: fix it so we will be using AddChild() and not deferred

		parent.CallDeferred(Node.MethodName.AddChild, enemy);

		EmitSignal(SignalName.EnemyCounterIncrease);
	}
}