using Godot;
using System;

public partial class Game : Node2D
{
	public Node CurrentLevel { get; set; }
	Player Player { get; set; }

	public override void _Ready()
	{
		Player = GetNode<Player>("/root/Player");

		CurrentLevel = ResourceLoader.Load<PackedScene>("res://Scenes/Levels/Level1.tscn").Instantiate();
		GetTree().Root.CallDeferred("add_child", CurrentLevel);

		Node2D playerSpawn = CurrentLevel.GetNode<Node2D>("PlayerSpawn");
		Player.Position = playerSpawn.Position;

		Player.PlayerDied += OnPlayerDied;
	}

	public void SetLevel() 
	{
		GetTree().Root.CallDeferred("add_child", CurrentLevel);
	}

	public void OnPlayerDied() 
	{
		GD.Print("The player died");
		// GetTree().ReloadCurrentScene();

		GetTree().Root.CallDeferred("remove_child", CurrentLevel);
		CurrentLevel = ResourceLoader.Load<PackedScene>("res://Scenes/Levels/Level1.tscn").Instantiate();
		GetTree().Root.CallDeferred("add_child", CurrentLevel);

		Player._Ready();
		Node2D playerSpawn = CurrentLevel.GetNode<Node2D>("PlayerSpawn");
		Player.Position = playerSpawn.Position;
	}
}
