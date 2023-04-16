using System.Numerics;
using System.Text.RegularExpressions;

namespace SeaBattle.Players;

public class HumanPlayer : SeaBattle.IPlayer
{
	public string Name { get; set; }
	public Map DefenseMap;
	public Map AttackMap;
	private IntegerVector2 cursorPosition = new IntegerVector2(0, 0);
	public int MapSeed { get; set; }

	public HumanPlayer(string name = "Player", int mapSeed = 0)
	{
		Name = name;
		MapSeed = mapSeed;
		DefenseMap = new Map(true, useLastHit:true);
		AttackMap = new Map(false, true);
	}

	private IntegerVector2 ReadInput()
	{
		Console.Write("Enter coordinates to attack: ");
		string input = Console.ReadLine ();
		string pattern = @"^([A-Za-z]+)(\d+)$"; // pattern to match one or more letters followed by one or more digits
		Match match = Regex.Match(input, pattern);
		if (!match.Success)
		{
			return new IntegerVector2(-1, -1);
		}
		else
		{
			string column = match.Groups[1].Value.ToUpper ();
			int row = int.Parse(match.Groups[2].Value) - 1;
			int columnLength = column.Length;
			int columnNumber = 0;
			for (int i = 0; i < columnLength; i++)
			{
				columnNumber += (column[i] - 'A' + 1) * (int) Math.Pow(26, columnLength - i - 1);
			}

			return new IntegerVector2(row, --columnNumber);
		}
	}

	private IntegerVector2 ReadArrowInput()
	{
		while (true)
		{
			Console.Clear ();
			DefenseMap.RenderMap ();
			AttackMap.RenderMap ();

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
					if (AttackMap.Grid[cursorPosition.X, cursorPosition.Y].IsAlreadyDestroyed ())
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
	public Map GetDefenseMap()
	{
		return DefenseMap;
	}

	public Map GetAttackMap()
	{
		return AttackMap;
	}

	public IntegerVector2 GetTarget(Cell[,] attackMap)
	{
		return ReadArrowInput ();
	}
	
	public override string ToString()
	{
		return "Human";
	}


	public string GetName()
	{
		return Name;
	}
}