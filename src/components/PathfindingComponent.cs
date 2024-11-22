using Godot;

namespace Game.Components;

public partial class PathfindingComponent : NavigationAgent2D
{
	[Export] private VelocityComponent velocityComponent;
	private Node2D _target;
	
	public void SetTarget(Node2D target)
	{
		_target = target;
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
