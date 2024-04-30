using Godot;
using System;

public partial class Level1 : Node2D
{
	LevelEntrance level2Entrance;
	DirectionalLight2D worldLight;

	public override void _Ready()
	{
		worldLight = GetNode<DirectionalLight2D>("WorldLight");
		worldLight.Energy = 12;
		level2Entrance = GetNode<LevelEntrance>("Level2Entrance");
		level2Entrance.NextLevel = ResourceLoader.Load<PackedScene>("res://Scenes/Levels/Level2.tscn").Instantiate();
	}
}
