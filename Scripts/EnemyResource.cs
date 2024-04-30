using Godot;
using System;

    
[GlobalClass]
public partial class EnemyResource : Resource
{
    [Export] 
    public float Speed { get; set; }
    [Export]
    public float HP { get; set; }
    [Export]
    public float Attack { get; set; }
    [Export]
    public float Defense { get; set; }
    [Export]
    public int GoldValue { get; set; }
            
    public EnemyResource() 
    {
        Speed = 100;
        HP = 100;
        GoldValue = 5;
    }
}
