namespace SeaBattle;

public class Configuration
{
	public const int size = 10;
	public static int seed = 0;
	
	public const int maxSmallShips = 4;
	public const int maxMediumShips = 3;
	public const int maxLargeShips = 2;
	public const int maxHugeShips = 1;

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