namespace SeaBattle;

public class Configuration
{
	public const int size = 10;
	public static int seed = 0;

	public const int maxSmallShips = 4;
	public const int maxMediumShips = 3;
	public const int maxLargeShips = 2;
	public const int maxHugeShips = 1;

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