using Game.Components;
using Godot;

namespace Game.Templates;

public partial class Enemy : CharacterBody2D
{
	[Export] public float AttackRange = 100f;
	[Export] public PathfindingComponent pathfindingComponent;
	[Export] public BbSimpleStateMachine bbSimpleStateMachine;
	[Export] public AnimatedSprite2D animatedSprite;
}