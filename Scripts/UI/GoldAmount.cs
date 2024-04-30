using Godot;
using System;

public partial class GoldAmount : Label
{
	int goldAmount = 0;

	public void OnGoldAmountChanged(int newGoldAmount) 
	{
		goldAmount = newGoldAmount;
		Text = newGoldAmount.ToString();
	}
}
