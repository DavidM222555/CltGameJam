using Godot;
using System;

public partial class Level2 : Node2D
{
		DirectionalLight2D worldLight;

	public override void _Ready()
	{
		worldLight = GetNode<DirectionalLight2D>("WorldLight");
		worldLight.Energy = 12;
	}
}
