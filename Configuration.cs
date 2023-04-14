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
		{ 0, new Pixel() { Color = ConsoleColor.White, Char = ' ' } },
		{ 1, new Pixel() { Color = ConsoleColor.DarkBlue, Char = 'S' } },
		{ 2, new Pixel() { Color = ConsoleColor.Blue, Char = 'M' } },
		{ 3, new Pixel() { Color = ConsoleColor.Cyan, Char = 'L' } },
		{ 4, new Pixel() { Color = ConsoleColor.DarkCyan, Char = 'H' } },
		{ 5, new Pixel() { Color = ConsoleColor.Red, Char = 'X' } },
		{ 6, new Pixel() { Color = ConsoleColor.Yellow, Char = 'x' } },
	};

	public static readonly Ships[] ShipArray = new Ships[4] { Ships.Small, Ships.Medium, Ships.Large, Ships.Huge };

	public enum Ships
	{
		Small = 1,
		Medium = 2,
		Large = 3,
		Huge = 4,
		
		None = 0,
		Destroyed = 5,
	}
}

public struct Pixel
{
	public ConsoleColor Color;
	public char Char;
}