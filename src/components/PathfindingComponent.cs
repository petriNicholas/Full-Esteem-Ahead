using Godot;

namespace Game.Components;

public partial class PathfindingComponent : NavigationAgent2D
{
    [Export] public VelocityComponent velocityComponent;

    private Node2D _target;
    private Node2D _agentNode;

    public override void _Ready()
    {
        _agentNode = GetParent<Node2D>();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_target != null)
        {
            this.TargetPosition = _target.GlobalPosition;
        }

        if (!IsNavigationFinished())
        {
            Vector2 direction = (GetNextPathPosition() - _agentNode.GlobalPosition).Normalized();
            velocityComponent.SetDirection(direction);
        }
        else
        {
            velocityComponent.SetDirection(Vector2.Zero);
        }
    }

    public void SetTarget(Node2D target)
    {
        _target = target;
    }

    public Vector2 GetDirection()
    {
        if (_target == null)
            return Vector2.Zero;

        this.TargetPosition = _target.GlobalPosition;

        if (!IsNavigationFinished())
        {
            return (GetNextPathPosition() - _agentNode.GlobalPosition).Normalized();
        }
        return Vector2.Zero;
    }
}
