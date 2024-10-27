using Godot;

public partial class Camera : Camera2D
{
	[Export] private CharacterBody2D player;
	[Export] private float cameraSpeed = 5.0f;         // Szybkość ruchu kamery
	[Export] private float mouseInfluence = 0.2f;      // Wpływ myszki na ruch kamery
	[Export] private Vector2 comfortZoneSize = new Vector2(48, 48); // Rozmiar strefy komfortu

	private Vector2 screenSize;

	public override void _Ready()
    {
        base._Ready();
		this.DragHorizontalEnabled = true;
		this.DragVerticalEnabled = true;

		screenSize = GetViewportRect().Size;
	}
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		UpdateCamera((float)delta);
	}

	private void UpdateCamera(float delta)
	{

	}
}
