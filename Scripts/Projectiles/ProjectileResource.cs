
using Godot;

[GlobalClass]
public partial class ProjectileResource : Resource {
    [Export]
    public float Speed { get; set; }
    [Export]
    public float Damage { get; set; }
    [Export]
    public float RotationSpeed { get; set; }

    public ProjectileResource() 
    {
        Speed = 50;
        Damage = 30;
        RotationSpeed = 0;
    }
    
}