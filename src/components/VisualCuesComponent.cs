using Godot;

namespace Game.Components;

public partial class VisualCuesComponent : Node2D
{
	private Label _label;

	private float _riseSpeed = 50f;
	private float _fadeSpeed = 1f;
	private Color _color;

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");

		if (_label == null)
		{
			AddChild(_label);
			GD.PrintErr("Label node could not be found!");
		}

		_color = _label.Modulate;
    }

	public void ShowDamage(int damageAmount, Vector2 position)
	{
		if (_label == null || !IsInstanceValid(_label))
		{
			GD.PrintErr("Label does not exist");
			return;
		}

		_label.Text = damageAmount.ToString();

		position.Y += 16;

		Position = position;

		_color.A = 1;
		_label.Modulate = _color;
	}

    public override void _Process(double delta)
    {
        Position += new Vector2(0, -_riseSpeed * (float)delta);

		_color.A -= _fadeSpeed * (float)delta;
		_label.Modulate = _color;

		if (_color.A <= 0 && IsInstanceValid(_label))
		{
			GD.PrintErr("queue free");
			QueueFree();
		}
    }
}