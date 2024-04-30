using Godot;
using System;

public partial class Gold : Item
{
	public int Amount { get; set; } = 1;

	public Gold() 
	{}

    public override void _Ready()
    {
        GD.Print("Gold created");
    }

    public Gold(int amount) 
	{
		Amount = amount;
	}

    public override void OnPickup(Player player)
    {
		player.HandleGoldPickup(Amount);
		QueueFree();
    }
}
