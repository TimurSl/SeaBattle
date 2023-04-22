using SeaBattle.Cells;
using SeaBattle.MapCreators.Types;
using SeaBattle.Types;

namespace SeaBattle.MapCreators;

public static class ManualMapCreator
{
	private static int currentObjectIndex = 0;
	public static Cell[,] GetLevel()
	{
		var map = new Map (LevelCreationType.Empty, false, false);
		var input = new IntegerVector2(0, 0);
		
		
		
		return map.Grid;
	}

	private static IntegerVector2 ReadMovementInput()
	{
		var input = Console.ReadKey(true);
		switch (input.Key)
		{
			case ConsoleKey.UpArrow:
				return new IntegerVector2(0, -1);
			case ConsoleKey.DownArrow:
				return new IntegerVector2(0, 1);
			case ConsoleKey.LeftArrow:
				return new IntegerVector2(-1, 0);
			case ConsoleKey.RightArrow:
				return new IntegerVector2(1, 0);
			default:
				return new IntegerVector2(0, 0);
		}
	}
	
	private static void ReadObjectInput()
	{
		var input = Console.ReadKey(true);
		switch (input.Key)
		{
			case ConsoleKey.PageUp:
				currentObjectIndex++;
				break;
			case ConsoleKey.PageDown:
				currentObjectIndex--;
				break;
			default:
				break;
		}
	}
}