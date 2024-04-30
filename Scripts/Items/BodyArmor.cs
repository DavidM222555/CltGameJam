using Godot;
using System;

public partial class BodyArmor : Item
{
	public new  void OnPickupRadiusEntered(Node2D enteringBody) 
    {
        GD.Print("Pickup radius being entered for gear: " + enteringBody);

        if (enteringBody is Player player) 
        {
            OnPickup(player);
        }
    }

    public override void OnPickup(Player player)
    {
        GD.Print("OnPickup being called for body armor");
		Visible = false;
		PickupRadius.SetDeferred("monitoring", false);
		PickupRadius.SetDeferred("monitorable", false);

		GetParent().CallDeferred("remove_child", this);
		player.CallDeferred("add_child", this);
		// Position = player.Position;

		player.BodyArmor = this;
    }
}
