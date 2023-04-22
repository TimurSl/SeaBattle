using SeaBattle.Players.Interfaces;
using SeaBattle.Settings;
using SeaBattle.Types;

namespace SeaBattle.Players.Inputs.Bot;

public class HardBotInput : IInput
{
	public IntegerVector2 HardGetTarget(Player attacker, Player enemy)
	{
		Random random = new Random();
		// get random point in enemy defense, get random ship with 10% chance, get random cell of this ship
		int x = random.Next(Configuration.size);
		int y = random.Next(Configuration.size);
		if (random.Next(0, 100) <= 60)
		{
			while (enemy.DefenseMap.Grid[x,y].CellType != Configuration.CellType.Ship)
			{
				x = random.Next(Configuration.size);
				y = random.Next(Configuration.size);
			}
		}
		
		attacker.AttackMap.SetCursorPosition(new IntegerVector2(x, y));
		return new IntegerVector2(x, y);
	}

	public IntegerVector2 GetCoordinates(Player attacker, Player defender)
	{
		return HardGetTarget(attacker, defender);
	}
}