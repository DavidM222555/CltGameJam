using Godot;
using System;
using UnitInterfaces;

public partial class Enemy : CharacterBody2D, IDamagable
{
	[Export]
	public Resource Stats;
	// Gathered from Stats
	private float speed;
	private float maxHp;
	private float hp;
	private int goldValue;
	private float attack;
	private Player player;
	private bool playerSpotted;
	private bool playerInRange;
	private Timer attackWindupTimer;
	private Sprite2D sprite;
	private HurtboxComponent hurtboxComponent;
	private ProgressBar healthBar;

	private bool flippedUpwardsRecently = false;
	private bool flippedDownwardsRecently = false;


	public override void _Ready()
	{
		attackWindupTimer = GetNode<Timer>("AttackWindup");

		player = GetNode<Player>("/root/Player");

		sprite = GetNode<Sprite2D>("Sprite2D");
		hurtboxComponent = GetNode<HurtboxComponent>("HurtboxComponent");

		if (Stats is EnemyResource enemyResource) 
		{
			speed = enemyResource.Speed;
			maxHp = enemyResource.HP;
			hp = enemyResource.HP;
			attack = enemyResource.Attack;
			goldValue = enemyResource.GoldValue;
		}

		healthBar = GetNode<ProgressBar>("EnemyHealthBar");
		healthBar.MaxValue = maxHp;
		healthBar.Value = hp;
		healthBar.SetPosition(new Vector2(Position.X - 15, Position.Y - 30));
	}

    public override void _PhysicsProcess(double delta)
    {
		if (playerSpotted) 
		{
			var playerPosition = player.Position;
			var targetPosition = (playerPosition - Position).Normalized();
			Velocity = targetPosition * speed;

			var absRotation = Math.Abs(Mathf.RadToDeg(Rotation));

			if (absRotation > 90 && absRotation < 180 && !flippedUpwardsRecently) 
			{
				Scale = new Vector2(Scale.X, -1 * Scale.Y);
				flippedUpwardsRecently = true;
				flippedDownwardsRecently = false;
			} else if ((absRotation <= 90 || absRotation >= 180) && !flippedDownwardsRecently) {
				Scale = new Vector2(Scale.X, -1 * Scale.Y);
				flippedDownwardsRecently = true;
				flippedUpwardsRecently = false;
			}

			LookAt(playerPosition);
			MoveAndSlide();
		}

		healthBar.SetPosition(new Vector2(Position.X - 15, Position.Y - 30));
    }

    public void OnDetectionBoxEntered(Node2D body) 
	{
		if (body is Player) 
		{
			playerSpotted = true;
		}
	}

	public void OnHitboxEntered(Node2D body) 
	{
		if (body is HurtboxComponent && body == player.Hurtbox) 
		{
			attackWindupTimer.Start();
			playerInRange = true;
		}
	}

	public void OnHitboxExited(Node2D body) 
	{
		GD.Print("In this guy for exiting hitbox");

		if (body is HurtboxComponent && body == player.Hurtbox) 
		{
			playerInRange = false;
		}
	}

	public void OnAttackWindupTimeout() 
	{
		// If the player is in range when the windup is done we land attack 
		// and handle player damage callback
		if (playerInRange) 
		{
			player.TakeDamage(attack);
			attackWindupTimer.Start();
		} else {
			attackWindupTimer.Stop();
		}
	}

	public void TakeDamage(float dmg) 
	{
		hp -= dmg; 
		healthBar.Value = hp;

		GD.Print("HP value: " + hp);
		GD.Print("Value in health bar: "  + healthBar.Value);

		if (hp <= 0) 
		{
			HandleDeath();
		}
	}

		// @Purpose: Takes care of things such as dropping gold when enemy dies, any future death animations,
	// and freeing the data associated with the enemy
	private void HandleDeath() 
	{
		// Get the current level which will always be the parent
		var currentLevel = GetParent<Node2D>();
		Gold gold = (Gold) GD.Load<PackedScene>("res://Scenes/Items/Gold.tscn").Instantiate();
		gold.Position = Position;
		gold.Amount = goldValue;

		currentLevel.CallDeferred("add_child", gold);

		QueueFree();
	}

}


