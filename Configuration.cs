namespace SeaBattle;

public class Configuration
{
	public const int size = 10;
	public static int seed = 0;
	
	public const int maxSmallShips = 4;
	public const int maxMediumShips = 3;
	public const int maxLargeShips = 2;
	public const int maxHugeShips = 1;
	
	public static readonly Dictionary<int, Pixel> PixelMap = new Dictionary<int, Pixel>()
	{
		{ (int) Ships.Nothing, new Pixel() { Color = ConsoleColor.White, Char = '_' } },
		{ (int) Ships.Small, new Pixel() { Color = ConsoleColor.DarkBlue, Char = 'S' } },
		{ (int) Ships.Medium, new Pixel() { Color = ConsoleColor.Blue, Char = 'M' } },
		{ (int) Ships.Large, new Pixel() { Color = ConsoleColor.Cyan, Char = 'L' } },
		{ (int) Ships.Huge, new Pixel() { Color = ConsoleColor.DarkCyan, Char = 'H' } },
		{ (int) Ships.Hit, new Pixel() { Color = ConsoleColor.Red, Char = 'X' } },
		{ (int) Ships.Miss, new Pixel() { Color = ConsoleColor.Yellow, Char = 'x' } },
	};

	public static readonly Ships[] ShipArray = new Ships[4] { Ships.Small, Ships.Medium, Ships.Large, Ships.Huge };

	public enum Ships
	{
		Nothing = 0,
		
		Small = 1,
		Medium = 2,
		Large = 3,
		Huge = 4,
		
		Miss = -1,
		Hit = -2,
	}
}

public struct Pixel
{
	public ConsoleColor Color;
	public char Char;
}