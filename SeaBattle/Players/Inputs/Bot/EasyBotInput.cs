using SeaBattle.Players.Interfaces;
using SeaBattle.Settings;
using SeaBattle.Types;

namespace SeaBattle.Players.Inputs.Bot;

public class EasyBotInput : IInput
{
	public IntegerVector2 EasyGetTarget(Player attacker, Player enemy)
	{
		Random random = new Random();
		int x = random.Next(Configuration.size);
		int y = random.Next(Configuration.size);
		while (attacker.AttackMap.Grid[x,y].IsAlreadyHit ())
		{
			x = random.Next(Configuration.size);
			y = random.Next(Configuration.size);
		}
		
		attacker.AttackMap.SetCursorPosition(new IntegerVector2(x, y));

		return new IntegerVector2(x, y);
	}

	public IntegerVector2 GetCoordinates(Player attacker, Player defender)
	{
		return EasyGetTarget(attacker, defender);
	}
}