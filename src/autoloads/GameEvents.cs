using Game.Components;
using Godot;

namespace Game.Autoload;

public partial class GameEvents : Node
{
	public static GameEvents Instance { get; private set; }

	[Signal]
	public delegate void DiedEventHandler(HealthComponent healthComponent);

	public override void _Notification(int what)
	{
		if (what == NotificationSceneInstantiated)
		{
			Instance = this;
		}
	}

	public static void EmitDied(HealthComponent healthComponent)
	{
		Instance.EmitSignal(SignalName.Died, healthComponent);
	}
}
