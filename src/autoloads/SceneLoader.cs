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
        // zmiana sceny
        GetTree().ChangeSceneToFile(targetScenePath);

        // poczekaj jedną klatkę aż nowa scena się załaduje
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        // załaduj gracza
        var playerScene = GD.Load<PackedScene>(playerScenePath);
        if (playerScene == null)
        {
            GD.PrintErr($"Nie udało się załadować sceny gracza: {playerScenePath}");
            return;
        }

        var player = playerScene.Instantiate<Node2D>();

        // weź główny root nowej sceny
        var currentScene = GetTree().CurrentScene;
        if (currentScene == null)
        {
            GD.PrintErr("Brak aktywnej sceny po zmianie!");
            return;
        }

        // dodaj gracza do nowej sceny
        currentScene.AddChild(player);

        // opcjonalnie ustaw startową pozycję (np. drzwi startowe, spawn point)
        // tutaj "PlayerSpawn" to Node2D w docelowej scenie
        var spawn = currentScene.GetNodeOrNull<Node2D>("PlayerSpawn");
        if (spawn != null)
        {
            player.Position = spawn.Position;
        }
    }
}
