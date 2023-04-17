namespace SeaBattle;

public class Configuration
{
	public const int size = 10;
	
	public static readonly Dictionary<int, Pixel> PixelMap = new()
	{
		{ (int) CellType.Nothing, new Pixel { Color = ConsoleColor.White, Char = '_' } },
		{ (int) CellType.Ship, new Pixel { Color = ConsoleColor.DarkBlue, Char = 'S' } },
		{ (int) CellType.Hit, new Pixel { Color = ConsoleColor.Red, Char = 'X' } },
		{ (int) CellType.Miss, new Pixel { Color = ConsoleColor.Yellow, Char = 'x' } }
	};
	public static readonly Dictionary<int, Pixel> ShipPixelMap = new()
	{
		{ (int) ShipType.Small, new Pixel { Color = ConsoleColor.DarkBlue, Char = 'S' } },
		{ (int) ShipType.Medium, new Pixel { Color = ConsoleColor.Blue, Char = 'M' } },
		{ (int) ShipType.Large, new Pixel { Color = ConsoleColor.Cyan, Char = 'L' } },
		{ (int) ShipType.Huge, new Pixel { Color = ConsoleColor.DarkCyan, Char = 'H' } },
	};

	public static readonly Dictionary<ShipType, ShipConfiguration> ShipConfigurations = new()
	{
		{ ShipType.Small, new ShipConfiguration { length = 1, count = 4 } },
		{ ShipType.Medium, new ShipConfiguration { length = 2, count = 3 } },
		{ ShipType.Large, new ShipConfiguration { length = 3, count = 2 } },
		{ ShipType.Huge, new ShipConfiguration { length = 4, count = 1 } }
	};

	public enum CellType
	{
		Nothing = 0,

		Ship = 1,

		Miss = -1,
		Hit = -2
	}
	
	public enum ShipType
	{
		Small = 1,
		Medium = 2,
		Large = 3,
		Huge = 4
	}
}

public struct Pixel
{
	public ConsoleColor Color;
	public char Char;
}

public struct ShipConfiguration
{
	public int length;
	public int count;
}