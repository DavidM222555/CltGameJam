using Godot;
using System;

public partial class Projectile : Node2D
{
	[Export]
	public bool EnemyHittable { get; set; }
	[Export]
	public bool PlayerHittable { get; set; }
	[Export]
	public bool DestroyedByTerrain { get; set; }
	[Export]
	public Vector2 Direction { get; set; }
	[Export]
	public Resource Stats { get; set; }

	public float Speed { get; set; }
	public float Damage { get; set; }
	public float RotationSpeed { get; set; }


    public override void _Ready()
    {
		EnemyHittable = true;
		PlayerHittable = true;
		DestroyedByTerrain = true;

		if (Stats is ProjectileResource projectileResource) 
		{
			Speed = projectileResource.Speed;
			Damage = projectileResource.Damage;
			RotationSpeed = projectileResource.RotationSpeed;
		}
    }

    public override void _PhysicsProcess(double delta)
    {
        base._Process(delta);

		Position = new Vector2((float)(Position.X + delta * Speed * Direction.X), (float)(Position.Y + delta * Speed * Direction.Y));
		Rotation += (float)( RotationSpeed * delta );
    }

	private void HandleProjectileDestruction() 
	{
		QueueFree();
	}

	public void OnHitboxEntered(Node2D enteringNode)  
	{
		if (enteringNode is Player player && PlayerHittable) 
		{
			player.TakeDamage(Damage);
			HandleProjectileDestruction();
		} 
		else if (enteringNode is Enemy enemy && EnemyHittable) 
		{
			enemy.TakeDamage(Damage);
			HandleProjectileDestruction();
		} 
		else if (enteringNode is StaticBody2D && DestroyedByTerrain) 
		{
			HandleProjectileDestruction();
		}
	}
}
