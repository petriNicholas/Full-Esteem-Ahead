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
	
	public async void ChangeSceneWithFade(string path)
	{
		var fade = GetNode<CanvasLayer>("Fade");
		fade.Show();
		await ToSignal(GetTree().CreateTimer(0.5), "timeout");

		GetTree().ChangeSceneToFile(path);
	}
}
