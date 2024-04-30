using Godot;
using ItemInterfaces;

public partial class Item : Node2D, IPickupable
{	
    [Export]
    bool AutoPickup { get; set; } = false;

    public Area2D PickupRadius { get; set; }

    public override void _Ready()
    {
        base._Ready();
        PickupRadius = GetNode<Area2D>("PickupRadius"); 
    }

    public void OnPickupRadiusEntered(Node2D enteringBody) 
    {
        GD.Print("Pickup radius being entered: " + enteringBody);

        if (enteringBody is Player player) 
        {
            OnPickup(player);
        }
    }

    public void OnPickupRadiusExited(Node2D exitingBody) 
	{
        GD.Print("Body being exited ");

		// if (exitingBody is Player player) 
		// {

		// }
	}

    public virtual void OnPickup(Player player) 
    {
    }
}
