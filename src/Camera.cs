using Godot;
using System;

public partial class Camera : Camera2D
{
	[Export] private CharacterBody2D player;
	[Export] private float cameraSpeed = 5.0f;         // Szybkość ruchu kamery
	[Export] private float mouseInfluence = 0.2f;      // Wpływ myszki na ruch kamery
	[Export] private Vector2 comfortZoneSize = new Vector2(0, 0); // Rozmiar strefy komfortu

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
		Vector2 mousePosition = GetGlobalMousePosition();

		Vector2 playerPosition = player.GlobalPosition;

		Vector2 cameraPosition = GlobalPosition;

		Vector2 comfortZoneMin = cameraPosition - comfortZoneSize / 2;
		Vector2 comfortZoneMax = cameraPosition + comfortZoneSize / 2;

		// Sprawdź, czy gracz wykracza poza strefę komfortu
		if (playerPosition.X < comfortZoneMin.X || playerPosition.X > comfortZoneMax.X ||
			playerPosition.Y < comfortZoneMin.Y || playerPosition.Y > comfortZoneMax.Y)
		{
			// Przesuń kamerę w kierunku gracza, jeśli wykracza poza strefę komfortu
			Vector2 targetPosition = playerPosition;
			GlobalPosition = GlobalPosition.Lerp(targetPosition, cameraSpeed * delta);
		}

		Vector2 mouseOffset = (mousePosition - screenSize / 2) * mouseInfluence;

		// Przesuń kamerę w kierunku myszki
		GlobalPosition += mouseOffset * delta;

	}
}
