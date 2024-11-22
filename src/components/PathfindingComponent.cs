using Godot;

namespace Game.Components;

public partial class PathfindingComponent : NavigationAgent2D
{
	[Export] public VelocityComponent velocityComponent;
	private Node2D _target;
	
	public void SetTarget(Node2D target)
	{
		_target = target;
	}
	
	public Vector2 GetDirection()
    {
        if (_target == null)
            return Vector2.Zero;

        TargetPosition = _target.GlobalPosition;  // Ustawiamy pozycję docelową (gracza)

        if (!IsNavigationFinished())
        {
            Vector2 agentPosition = GetParent<Node2D>().GlobalPosition;
            return (GetNextPathPosition() - agentPosition).Normalized();  // Obliczamy normalizowany kierunek
        }
        return Vector2.Zero;  // Zwracamy Zero, jeśli już nie ma drogi do przebycia
    }

	public override void _Process(double delta)
	{
		if(_target != null)
		{
			TargetPosition = _target.GlobalPosition;
		}

		if (!IsNavigationFinished())
        {
			Vector2 agentPosition = GetParent<Node2D>().GlobalPosition;
            Vector2 direction = (GetNextPathPosition() - agentPosition).Normalized();
            velocityComponent.SetDirection(direction);
        }
        else
        {
            velocityComponent.SetDirection(Vector2.Zero);
        }
	}
}
