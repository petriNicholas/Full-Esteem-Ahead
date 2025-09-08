using Game.Autoload;
using Godot;

namespace Game.Scenes;

public partial class Door : Area2D
{
	[Export] public string TargetScenePath = "";
    [Export] public string PlayerScenePath = "";

    private bool _playerInside = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node2D body)
    {
        _playerInside = true;
    }

    private void OnBodyExited(Node2D body)
    {
        _playerInside = false;
    }

    public override void _Process(double delta)
    {
        if (_playerInside && Input.IsActionJustPressed("interact"))
        {
            if (!string.IsNullOrEmpty(TargetScenePath))
            {
                SceneLoader.Instance.ChangeSceneAndSpawnPlayer(TargetScenePath, PlayerScenePath);
            }
        }
    }
}
