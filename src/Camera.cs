using Godot;
using System;

public partial class Camera : Camera2D
{
	[Export] private Node2D playerPath;
	[Export] private float cameraSpeed = 5.0f;         // Szybkość ruchu kamery
	[Export] private float mouseInfluence = 0.2f;      // Wpływ myszki na ruch kamery
	[Export] private Vector2 comfortZoneSize = new Vector2(100, 100); // Rozmiar strefy komfortu

	private Node2D player;                             // Referencja do gracza
	private Vector2 screenSize;

	public override void _Ready()
    {
        base._Ready();
		this.DragHorizontalEnabled = true;
		this.DragVerticalEnabled = true;
		CharacterBody2D player = GetParent<CharacterBody2D>();

		// Rozmiar ekranu
		screenSize = GetViewportRect().Size;
	}
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		UpdateCamera((float)delta);
	}

	private void UpdateCamera(float delta)
	{
		// Pozycja gracza
		Vector2 playerPosition = player.GlobalPosition;

		// Pozycja kamery
		Vector2 cameraPosition = GlobalPosition;

		// Oblicz strefę komfortu (prostokąt na środku ekranu)
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

		// Wpływ myszki na kamerę
		Vector2 mousePosition = GetViewport().GetMousePosition();
		Vector2 mouseOffset = (mousePosition - screenSize / 2) * mouseInfluence;

		// Przesuń kamerę w kierunku myszki
		GlobalPosition += mouseOffset * delta;
	}
}
