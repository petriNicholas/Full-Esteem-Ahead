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

	public async void ChangeSceneAndSpawnPlayer(string pathToScene, string pathToPlayerScene)
	{
		// 1. Wczytaj nową scenę
		var err = GetTree().ChangeSceneToFile(pathToScene);

		// 2. Poczekaj 1 klatkę aż scena się załaduje
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		// 3. Znajdź spawn point
		var currentScene = GetTree().CurrentScene;
		var spawnPoint = currentScene.GetNodeOrNull<Node2D>("PlayerSpawnPoint");

		if (spawnPoint == null)
		{
			GD.PushError("PlayerSpawn not found in loaded scene.");
			return;
		}

		// 4. Załaduj i dodaj gracza
		var playerScene = GD.Load<PackedScene>(pathToPlayerScene);
		var player = playerScene.Instantiate<Node2D>();

		currentScene.AddChild(player);
		player.GlobalPosition = spawnPoint.GlobalPosition;
	}
}
