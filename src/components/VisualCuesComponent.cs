using Godot;
using System;

namespace Game.Components;

public partial class VisualCuesComponent : Node2D
{
	public Label _label;

	private float _riseSpeed = 50f;
	private float _fadeSpeed = 2f;
	private Color _color;

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");

		_color = _label.Modulate;
    }

	public void ShowDamage(int damageAmount, Vector2 position)
	{
		if (_label == null || !IsInstanceValid(_label))
		{
			_label = new Label();
			AddChild(_label);
		}

		_label.Text = damageAmount.ToString();
		
		Random temp = new Random();

		_label.SetPosition(new Vector2(temp.Next(-10, 10), temp.Next(-10, 10)));

		if (damageAmount < 0) _color.G = 1;
		else if (damageAmount > 0) _color.R = 1;
		_label.LabelSettings.FontSize = 12;
		position.Y -= 16;
		Position = position;

		_color.A = 1;
		_label.Modulate = _color;
	}

    public override void _Process(double delta)
    {
        Position += new Vector2(Position.X, -_riseSpeed * (float)delta);

		_color.A -= _fadeSpeed * (float)delta;
		_label.Modulate = _color;

		if (_color.A <= 0 && IsInstanceValid(_label))
		{
			QueueFree();
		}
	}
}