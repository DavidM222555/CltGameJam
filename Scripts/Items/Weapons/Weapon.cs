using Godot;
using System;
using System.Collections.Generic;

public partial class Weapon : Item
{
	public AnimationPlayer Animation { get; set; }
	public Sprite2D Sprite2D { get; set; }
	public Area2D Hitbox { get; set; }
	public float Damage { get; set; }
	public Label PickupLabel { get; set; }
	public Player Player { get; set; }
	
	private bool playerInRange;

	// @Purpose: Used for keeping track of which enemies were hit in one rotation of the weapon
	public ISet<Enemy> HitEnemies { get; set; }


	public override void _Ready()
	{
		Animation = GetNode<AnimationPlayer>("AnimationPlayer");
		PickupRadius = GetNode<Area2D>("PickupRadius"); 
		Hitbox = GetNode<Area2D>("HitboxComponent");
		Sprite2D = GetNode<Sprite2D>("Sprite2D");

		PickupLabel = GetNode<Label>("Label");
		PickupLabel.SetPosition(new Vector2(Position.X - 40, Position.Y + 20));
		PickupLabel.Visible = false;

		Damage = 20;
		HitEnemies = new HashSet<Enemy>();

		playerInRange = false;
	}

    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("E") && playerInRange && Player != null) 
		{
			GD.Print("Handling player clicking E");
			OnPickup(Player);
		}
    }

    private void OnAnimationFinished(string animationName) 
	{
		Visible = false;
		Animation.Stop();
		Hitbox.Monitorable = false;
		Hitbox.Monitoring = false;
		HitEnemies = new HashSet<Enemy>();
	}

	private void OnHitboxCollided(Node2D collidedObject) 
	{
		if (collidedObject is Enemy enemy && !HitEnemies.Contains(enemy)) 
		{
			enemy.TakeDamage(Damage);
			HitEnemies.Add(enemy);
		}
	}

	public new void OnPickupRadiusEntered(Node2D enteringBody) 
    {
        GD.Print("Pickup radius being entered: " + enteringBody);

        if (enteringBody is Player player) 
        {
			PickupLabel.Visible = true;
			playerInRange = true;
			Player = player;
            // OnPickup(player);
        }
    }

	public new void OnPickupRadiusExited(Node2D exitingBody) 
	{
		GD.Print("Calling exit function");

		if (exitingBody is Player player) 
		{
			PickupLabel.Visible = false;
			playerInRange = false;
		}
	}

	public override void OnPickup(Player player)
    {
		GD.Print("OnPickup being called");

		Visible = false;
		PickupRadius.SetDeferred("monitoring", false);
		PickupRadius.SetDeferred("monitorable", false);

		GetParent().CallDeferred("remove_child", this);
		player.CallDeferred("add_child", this);
		Position = player.Position;

		player.Weapon = this;    
		PickupLabel.Visible = false;

		player.HandleWeaponChange();
	}
}
