using Godot;
using System;

public partial class ProjectileSpawner : Node2D
{
	[Export] 
	PackedScene projectileScene;
	[Export]
	double spawnWaitTime;
	[Export]
	Vector2 projectileDirection;

	Timer spawnRate;

    public override void _Ready()
    {
        base._Ready();
		spawnRate = GetNode<Timer>("SpawnRate");

		if (spawnWaitTime != 0.0) 
		{
			spawnRate.WaitTime = spawnWaitTime;
		} else {
			spawnRate.WaitTime = 3.0;
		}

		if (projectileDirection.X != 0 || projectileDirection.Y != 0) 
		{
			projectileDirection = new Vector2(projectileDirection.X, projectileDirection.Y);
		} else {
			projectileDirection = new Vector2(0, -1);
		}	 

		CallDeferred("SpawnProjectile");

		spawnRate.Start();
    }

	public void SpawnProjectile() 
	{
		Projectile projectile = (Projectile) projectileScene.Instantiate();
		projectile.Position = Position;
		GetParent().AddChild(projectile);

		projectile.Direction = projectileDirection;

	}

	public void OnSpawnRateTimeout() 
	{
		spawnRate.Start();
		SpawnProjectile();
	}
}
