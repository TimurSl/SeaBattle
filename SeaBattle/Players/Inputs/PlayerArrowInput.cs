using SeaBattle.MapCreators.Types;
using SeaBattle.Players.Interfaces;
using SeaBattle.Settings;
using SeaBattle.Types;

namespace SeaBattle.Players.Inputs;

public class PlayerArrowInput : IInput
{
	private IntegerVector2 cursorPosition = new IntegerVector2(0, 0);
	private Map DefenseMap;
	private Map AttackMap;
	
	
	private IntegerVector2 ReadArrowInput(Map defenseMap, Map attackMap)
	{
		DefenseMap = defenseMap;
		AttackMap = attackMap;
		while (true)
		{
			Console.Clear ();
			defenseMap.RenderMap ();
			attackMap.RenderMap ();

			ConsoleKeyInfo key = Console.ReadKey(true);

			switch (key.Key)
			{
				case ConsoleKey.UpArrow:
					MoveCursorTo(new IntegerVector2(-1, 0));
					break;
				case ConsoleKey.DownArrow:
					MoveCursorTo(new IntegerVector2(1, 0));
					break;
				case ConsoleKey.LeftArrow:
					MoveCursorTo(new IntegerVector2(0, -1));
					break;
				case ConsoleKey.RightArrow:
					MoveCursorTo(new IntegerVector2(0, 1));
					break;
				case ConsoleKey.Enter:
					if (AttackMap.Grid[cursorPosition.X, cursorPosition.Y].IsAlreadyHit ())
						break;
					return cursorPosition;
				case ConsoleKey.Spacebar:
					if (AttackMap.Grid[cursorPosition.X, cursorPosition.Y].IsAlreadyHit ())
						break;
					return cursorPosition;
			}
		}
	}
	
	private void MoveCursorTo(IntegerVector2 direction)
	{
		// check if cursor is in bounds
		if (cursorPosition.X + direction.X < 0 || cursorPosition.X + direction.X >= Configuration.size || cursorPosition.Y + direction.Y < 0 || cursorPosition.Y + direction.Y >= Configuration.size)
		{
			return;
		}
		cursorPosition += direction;
		AttackMap.SetCursorPosition(cursorPosition);
	}
	
	public IntegerVector2 GetCoordinates(Player attacker, Player defender)
	{
		return ReadArrowInput (attacker.DefenseMap, attacker.AttackMap);
	}
}