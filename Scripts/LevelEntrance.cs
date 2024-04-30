using Godot;
using System;

public partial class LevelEntrance : Area2D
{
	public Node NextLevel { get; set; }
	public Node CurrentLevel { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CurrentLevel = GetParent<Node2D>();
	}

	public void OnLevelEntranceEntered(Node2D enteringBody) 
	{
		if (enteringBody is Player player) 
		{
			player.Position = NextLevel.GetNode<Node2D>("PlayerSpawn").Position;
			var game = GetNode<Game>("/root/Game");

			game.CurrentLevel = NextLevel;
			game.SetLevel();
			CurrentLevel.QueueFree();
		}
	}
}
