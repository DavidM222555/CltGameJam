using Godot;
using System;

public partial class Mana : ProgressBar
{
	public void OnManaChanged(float newMana) 
	{
		Value = newMana;
	}

	public void OnMaxManaChanged(float newMaxMana) 
	{
		MaxValue = newMaxMana;
	}
}
