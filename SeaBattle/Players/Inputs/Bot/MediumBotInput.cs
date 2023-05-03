using SeaBattle.Players.Interfaces;
using SeaBattle.Settings;
using SeaBattle.Types;

namespace SeaBattle.Players.Inputs.Bot;

public class MediumBotInput : IInput
{
	public IntegerVector2 MediumGetTarget(Player attacker, Player enemy)
	{
		Random random = new Random();
		int x = random.Next(Configuration.size);
		int y = random.Next(Configuration.size);
		// try to find already hit (not miss) cell, and then try to find a cell around it (if no cells hit, then just random)
		int attempts = 10;
		bool found = false;
		while (attacker.AttackMap.Grid[x,y].CellType != Configuration.CellType.Hit && attempts > 0 && !attacker.AttackMap.Grid[x,y].InBounds ())
		{
			x = random.Next(Configuration.size);
			y = random.Next(Configuration.size);
			attempts--;
			if (enemy.DefenseMap.Grid[x,y].CellType == Configuration.CellType.Hit)
			{
				found = true;
			}
		}

		if (found)
		{
			int x1 = x + random.Next(-1, 2);
			int y1 = y + random.Next(-1, 2);
			
			// check if it is in bounds
			if (!attacker.AttackMap.Grid[x1,y1].IsAlreadyHit () && attacker.AttackMap.Grid[x1,y1].InBounds ())
			{
				x = x1;
				y = y1;
			}
		}
		
		attacker.AttackMap.SetCursorPosition(new IntegerVector2(x, y));
		return new IntegerVector2(x, y);
	}
	
	public IntegerVector2 GetCoordinates(Player attacker, Player defender)
	{
		return MediumGetTarget(attacker, defender);
	}
}