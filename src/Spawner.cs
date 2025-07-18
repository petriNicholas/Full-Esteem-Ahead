using System;
using System.Runtime.CompilerServices;
using Godot;

public partial class Spawner : Node2D
{
    [Signal] public delegate void EnemyCounterIncreaseEventHandler(int num);
    [Signal] public delegate void EnemyCounterDecreaseEventHandler(int num);
    [Signal] public delegate void SpawnNewEnemyEventHandler();

    [Export] private bool _endlessSpawn = false;
    [Export] private PackedScene _enemyScene;
    [Export] private int _enemyCnt = 0;
    [Export] private int _maxEnemyCnt = 10;
    [Export] private Marker2D _marker = new Marker2D();
    private Node _parent;
    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        _marker.Position = new Vector2(0, 0);
        _parent = GetParent();

        if (!_endlessSpawn)
        {
            this.EnemyCounterIncrease += CounterUpdate;
            this.EnemyCounterDecrease += CounterUpdate;
        }

        this.SpawnNewEnemy += SpawnEnemy;

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Vector2 randomOffset = new Vector2(
            _rng.Randf() * 10 - _rng.Randf() * 10,
            _rng.Randf() * 10 - _rng.Randf() * 10
        );

        var enemy = _enemyScene.Instantiate<CharacterBody2D>();

        enemy.Position = _marker.Position + randomOffset;

        // _parent.AddChild(enemy); // TODO: fix it so we will be using AddChild() and not deferred

        _parent.CallDeferred(Node.MethodName.AddChild, enemy);
        if (!_endlessSpawn)
        {
            EmitSignal(SignalName.EnemyCounterIncrease, 1);
        }
        else
        {
            EmitSignal(SignalName.SpawnNewEnemy);
        }
    }

    private void CounterUpdate(int num)
    {
        _enemyCnt += num;

        if (_enemyCnt < _maxEnemyCnt)
        {
            EmitSignal(SignalName.SpawnNewEnemy);
        }
    }
}