using Godot;

namespace Game.Autoload;

public partial class SceneLoader : Node
{
	public static SceneLoader Instance { get; private set; }

	public override void _Notification(int what)
	{
		if (what == NotificationSceneInstantiated)
		{
			Instance = this;
		}
	}

	public async void ChangeSceneAndSpawnPlayer(string targetScenePath, string playerScenePath)
    {
        GetTree().ChangeSceneToFile(targetScenePath);

        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        var playerScene = GD.Load<PackedScene>(playerScenePath);

        var player = playerScene.Instantiate<Node2D>();

        var currentScene = GetTree().CurrentScene;

        currentScene.AddChild(player);

        var spawn = currentScene.GetNodeOrNull<Marker2D>("PlayerSpawn");
        if (spawn != null)
        {
            player.Position = spawn.Position;
        }
    }
}
