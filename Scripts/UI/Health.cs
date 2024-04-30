using Godot;
using System;

public partial class Health : ProgressBar
{
    public void OnHPChanged(float newHP) 
	{
		Value = newHP;
	}

	public void OnMaxHPChanged(float newMaxHP) 
	{
		MaxValue = newMaxHP;
	}
}
