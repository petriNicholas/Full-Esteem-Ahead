using Godot;

namespace Game;

public partial class Bullet : Node2D
{
	[Export] public float _initColRadius { get; set; }
	[Export] public float Acceleration {get; set;} = 10.0f;
	[Export] public float Speed {get; set;} = 0;
	[Export] public float MaxSpeed {get; set;} = 0;
	[Export] public float Lifetime {get; set;} = 0;
	[Export] public float CheckBoundaryTime {get; set;} = 0;

	[Export] public float AngularSpeed = 0.0f;
	[Export] public float MaxAngularStray = 0.0f;
}