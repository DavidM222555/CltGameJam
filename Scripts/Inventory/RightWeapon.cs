using Godot;
using System;

public partial class RightWeapon : TextureRect
{
	public Sprite2D RightWeaponSprite; 

	public override void _Ready()
	{
		RightWeaponSprite = GetNode<Sprite2D>("RightWeaponSprite");
		Player player = GetNode<Player>("/root/Player");
		player.WeaponChanged += OnWeaponChanged;
	}

	public void OnWeaponChanged(Weapon weapon) 
	{
		GD.Print("In this guy changing weapon");
		RightWeaponSprite.Texture = weapon.Sprite2D.Texture;
	}

}
