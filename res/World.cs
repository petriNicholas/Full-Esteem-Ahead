using System;
using Game.Autoload;
using Game.Components;
using Godot;

public partial class World : Node
{
    public override void _Ready()
    {
        GameEvents.Instance.Died += OnDied;
    }

    private void OnDied(HealthComponent healthComponent)
    {
        GD.Print(healthComponent.IsAlive());
    }
}
