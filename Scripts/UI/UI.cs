using Godot;
using System;

public partial class UI : CanvasLayer
{
    public Health HealthBar { get; set; }
    public Stamina StaminaBar { get; set; }
    public Mana ManaBar { get; set; }
    public GoldAmount GoldAmountBar { get; set; }

    public override void _Ready() 
    {
        HealthBar = GetNode<Health>("%Health");
        StaminaBar = GetNode<Stamina>("%Stamina");
        ManaBar = GetNode<Mana>("%Mana");
        GoldAmountBar = GetNode<GoldAmount>("%GoldAmount");
    }
}
