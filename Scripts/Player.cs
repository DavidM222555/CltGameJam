using Godot;
using UnitInterfaces;

public partial class Player : CharacterBody2D, IDamagable
{
	[Export]
	public float Speed { get; set; } = 225.0f;
	public float MaxHP { get; set; }
	public float HP { get; set; }

	public float MaxStamina { get; set; }
	public float Stamina { get; set; }
	public float StaminaRefresh { get; set; }
	private Timer staminaRefreshTimer;

	public float MaxMana { get; set; }
	public float Mana { get; set; }
	public float ManaRefresh { get; set; }
	private Timer manaRefreshTimer;

	public int Gold { get; set; }
	public HurtboxComponent Hurtbox { get; set; }
	public Area2D meleeAttackRange;
	public float AttackCost { get; set; } = 10;
	public Weapon Weapon { get; set; }
	public BodyArmor BodyArmor { get; set; }
	private Camera2D camera;
	private UI ui;

    public override void _Ready()
    {
		Hurtbox = GetNode<HurtboxComponent>("HurtboxComponent");
		meleeAttackRange = GetNode<Area2D>("MeleeAttackRange");

		camera = GetNode<Camera2D>("Camera2D");

		// Set up UI signals		
		ui = GetNode<UI>("/root/UI");

		StaminaChanged += ui.StaminaBar.OnStaminaChanged;
		MaxStaminaChanged += ui.StaminaBar.OnMaxStaminaChanged;

		HPChanged += ui.HealthBar.OnHPChanged;
		MaxHPChanged += ui.HealthBar.OnMaxHPChanged;

		ManaChanged += ui.ManaBar.OnManaChanged;
		MaxManaChanged += ui.ManaBar.OnMaxManaChanged;

		GoldAmountChanged += ui.GoldAmountBar.OnGoldAmountChanged;
		// End of setting up UI signals

		MaxHP = 120;
		HP = MaxHP;
		EmitSignal(SignalName.MaxHPChanged, MaxHP);
		EmitSignal(SignalName.HPChanged, HP);

		MaxStamina = 100;
		Stamina = MaxStamina;
		StaminaRefresh = 5;
		staminaRefreshTimer = GetNode<Timer>("StaminaRefresh");
		staminaRefreshTimer.Start();
		EmitSignal(SignalName.MaxStaminaChanged, MaxStamina);
		EmitSignal(SignalName.StaminaChanged, Stamina);

		MaxMana = 100;
		Mana = MaxMana;
		ManaRefresh = 3;
		manaRefreshTimer = GetNode<Timer>("ManaRefresh");
		manaRefreshTimer.Start();
		EmitSignal(SignalName.MaxManaChanged, MaxMana);
		EmitSignal(SignalName.ManaChanged, Mana);

		Gold = 0;
		EmitSignal(SignalName.GoldAmountChanged, Gold);
    }

    public void GetInput() 
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		Velocity = inputDirection * Speed;
	}

    public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}

	public void TakeDamage(float dmg) 
	{
		HP -= dmg;

		if (HP <= 0) 
		{
			HP = MaxHP;
			EmitSignal(SignalName.PlayerDied);
			// QueueFree();
		}

		EmitSignal(SignalName.HPChanged, HP);
	}


	public void OnMeleeAttackRangeEvent(Viewport viewport, InputEvent @event, int shapeIdx) 
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.IsPressed()) 
		{	
			HandleAttack(GetLocalMousePosition());
		}
	}


	private void HandleAttack(Vector2 attackPos) 
	{
		if ((Stamina - AttackCost) >= 0 && Weapon != null) 
		{
			Stamina -= AttackCost;
			Weapon.Position = new Vector2(attackPos.X, attackPos.Y);

			Weapon.Visible = true;
			Weapon.Hitbox.Monitorable = true;
			Weapon.Hitbox.Monitoring = true;

			if (Weapon.Position.X < 0) {
				Weapon.Animation.Play("MeleeAttackBackward");
			} else {
				Weapon.Animation.Play("MeleeAttack");
			}

			EmitSignal(SignalName.StaminaChanged, Stamina);
		}
	}

	public void HandleWeaponChange() 
	{
		EmitSignal(SignalName.WeaponChanged, Weapon);
	}

	public void OnStaminaRefreshTimeout() 
	{
		if ((Stamina + StaminaRefresh) >= MaxStamina) 
		{
			Stamina = MaxStamina;
		} else if ((Stamina + StaminaRefresh) < MaxStamina) {
			Stamina += StaminaRefresh;
		}

		EmitSignal(SignalName.StaminaChanged, Stamina);
		staminaRefreshTimer.Start();
	}

	public void OnManaRefreshTimeout() 
	{
		if ((Mana + ManaRefresh) >= MaxMana) 
		{
			Mana = MaxMana;
		} else if ((Mana + ManaRefresh) < MaxMana) {
			Mana += ManaRefresh;
		}

		EmitSignal(SignalName.ManaChanged, Mana);
		manaRefreshTimer.Start();
	}

	public void HandleGoldPickup(int goldAmount) 
	{
		Gold += goldAmount;
		EmitSignal(SignalName.GoldAmountChanged, Gold);
	}

	[Signal]
	public delegate void WeaponChangedEventHandler(Weapon newWeapon);

	[Signal]
	public delegate void HPChangedEventHandler(float newHP);
	[Signal]
	public delegate void MaxHPChangedEventHandler(float newMaxHP);
	[Signal]
	public delegate void PlayerDiedEventHandler();

	[Signal]
	public delegate void StaminaChangedEventHandler(float newStamina);
	[Signal]
	public delegate void MaxStaminaChangedEventHandler(float newMaxStamina);

	[Signal]
	public delegate void ManaChangedEventHandler(float newMana);
	[Signal]
	public delegate void MaxManaChangedEventHandler(float newMaxMana);

	[Signal]
	public delegate void GoldAmountChangedEventHandler(int newGoldAmount);
}
