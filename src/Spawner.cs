using System;
using Godot;

public partial class Spawner : Node2D
{
	[Signal] public delegate void EnemyCounterIncreaseEventHandler();
	[Export] private PackedScene _enemyScene;
	[Export] private Marker2D _marker = new Marker2D();
	private Node _parent;
	private RandomNumberGenerator _rng = new RandomNumberGenerator();

	public override void _Ready()
	{
		_parent = GetParent();
		_marker.Position = new Vector2(0, 0);
		
		SpawnEnemies();
	}

	public void SpawnEnemies()
	{

		Vector2 randomOffset = new Vector2(
			_rng.Randf() * 10 - _rng.Randf() * 10,
			_rng.Randf() * 10 - _rng.Randf() * 10
		);
		
		var enemy = _enemyScene.Instantiate<CharacterBody2D>();

		enemy.Position = _marker.Position + randomOffset;

		// _parent.AddChild(enemy); // TODO: fix it so we will be using AddChild() and not deferred

		_parent.CallDeferred(Node.MethodName.AddChild, enemy);

		EmitSignal(SignalName.EnemyCounterIncrease);
	}
}