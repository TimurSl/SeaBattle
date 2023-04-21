using System.Text.RegularExpressions;

namespace SeaBattle.Players.Inputs;

public class PlayerStringInput : IInput
{
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



	public IntegerVector2 GetCoordinates(Player attackMap, Player enemyDefenseMap)
	{
		IntegerVector2 coordinates = ReadInput();
		while (!coordinates.InBounds ())
		{
			Console.WriteLine("Invalid coordinates");
			coordinates = ReadInput();
		}

		return coordinates;
	}
}