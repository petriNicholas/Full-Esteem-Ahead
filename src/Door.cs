using Godot;

public partial class Door : Area2D
{
	[Export] public string ScenePath = "";

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Game.Player)
        {
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("interact") && ScenePath != "")
        {
            GetTree().ChangeSceneToFile(ScenePath);
        }
    }
}
