using Godot;
using System;

public partial class Stamina : ProgressBar
{
	public void OnStaminaChanged(float newStamina) 
	{
		Value = newStamina;
	}

	public void OnMaxStaminaChanged(float newMaxStamina) 
	{
		MaxValue = newMaxStamina;
	}
}
