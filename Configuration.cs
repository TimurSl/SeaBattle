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

public class WaterObject {
	public int x;
	public int y;
	public WaterObjectTypes type;
	public char Char => Configuration.PixelMap[(int) type].Char;
    
	public WaterObject(int x, int y) {
		this.x = x;
		this.y = y;
		this.type = WaterObjectTypes.Air;
	}
}

public class Air : WaterObject
{
	public int x;
	public int y;
	public Air(int x, int y) : base(x, y)
	{
		this.x = x;
		this.y = y;
		this.type = WaterObjectTypes.Air;
	}
}

public enum WaterObjectTypes
{
	Air = 0,
	Ship = 1,
    
	Miss = -1,
	Hit = -2,
}

public enum ShipTypes
{
	SmallShip = 1,
	MediumShip = 2,
	LargeShip = 3,
	HugeShip = 4,
}

public class Ship : WaterObject {
	public int x;
	public int y;
	public int length;
	public bool isHorizontal;
	public ShipTypes shipType;
	public char Char => Configuration.PixelMap[(int) shipType].Char;
    
	public Ship(int x, int y, int length, bool isHorizontal, ShipTypes type) : base(x, y)
	{
		this.x = x;
		this.y = y;
		this.length = length;
		this.isHorizontal = isHorizontal;
		this.shipType = (ShipTypes) type;
		this.type = WaterObjectTypes.Ship;
	}
}
