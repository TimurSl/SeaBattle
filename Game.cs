using System.Numerics;
using System.Text.RegularExpressions;
using SeaBattle.Players;

namespace SeaBattle;


public class Game
{
	int[,] player1MapDefense;
	int[,] player1MapAttack;
	
	int[,] player2MapDefense;
	int[,] player2MapAttack;
	
	bool isPlayer1Turn = true;
	
	private IPlayer player1 = new Player();
	private IPlayer player2 = new Bot();
	

	public Game()
	{
		// Generate the two maps for the players
		LevelGenerator levelGenerator = new LevelGenerator();
		player1MapDefense = levelGenerator.GenerateLevel(Configuration.seed);
		player2MapDefense = levelGenerator.GenerateLevel(Configuration.seed + 1);

		// Initialize the attack maps for both players
		player1MapAttack = new int[Configuration.size, Configuration.size];
		player2MapAttack = new int[Configuration.size, Configuration.size];
	}

	public void Start()
	{
		Console.WriteLine(Figgle.FiggleFonts.Doom.Render("Sea Battle"));
		WelcomeMessage();

		Console.ReadKey();
		Console.Clear();
		
		// Loop until the game is over
		while (CanGameRun())
		{
			// Draw the maps
			DrawMap(player1MapDefense);
			Console.WriteLine();
			DrawMap(player1MapAttack);
			Console.WriteLine();
			DrawMap(player2MapDefense);
			
			// If it is the player's turn, then the player takes their turn
			if (isPlayer1Turn)
			{
				IntegerVector2 target = player1.GetTarget(player2MapDefense);
				
				if (target != new IntegerVector2(-1, -1))
				{
					int x = (int) target.X;
					int y = (int) target.Y;

					HitShip(target, player2MapDefense, player1MapAttack);
				}
				
				isPlayer1Turn = false;
			}
			// Otherwise, the enemy takes their turn
			else
			{
				IntegerVector2 target = player2.GetTarget(player1MapDefense);
				
				if (target != new IntegerVector2(-1, -1))
				{
					int x = (int) target.X;
					int y = (int) target.Y;

					HitShip(target, player1MapDefense, player2MapAttack);
				}

				isPlayer1Turn = true;
			}
			Console.Clear();
		}
	}

	private static void WelcomeMessage()
	{
		Console.WriteLine("Welcome to Sea Battle!");
		Console.WriteLine("You can skip your turn by pressing enter.");
		// help 
		Console.WriteLine("To attack, enter coordinates in the format A1, B2, C3, etc.");
		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("S - small ship");
		Console.WriteLine("M - medium ship");
		Console.WriteLine("L - large ship");
		Console.WriteLine("X - hit");
		Console.WriteLine("x - miss");
		Console.WriteLine();
		Console.ResetColor();

		Console.WriteLine("Press any key to start the game.");
	}


	bool CanGameRun()
	{
		for (int p = 1; p <= 2; p++)
		{
			int[,] playerMap = (p == 1) ? player1MapDefense : player2MapDefense;

			for (int x = 0; x < playerMap.GetLength(0); x++)
			{
				for (int y = 0; y < playerMap.GetLength(1); y++)
				{
					if (playerMap[x, y] > 1)
					{
						return true;
					}
				}
			}
		}
		
		return false;
	}
	
	private void DrawMap(int[,] map)
	{
		// draw coordinates (A-(level size in letters)) and numbers (1-(level size in numbers)), if levelsize > 26, use 2 letters for coordinates
		// draw letters
		DrawLetters(map);

		for (int x = 0; x < map.GetLength(0); x++)
		{
			DrawNumber(x);

			Console.Write("|");

			// numbers
			for (int y = 0; y < map.GetLength(1); y++)
			{
				ConsoleColor color = ConsoleColor.White;
				char @char = ' ';
				// draw map
				
				// get color and @char from Configuration.PixelMap
				color = Configuration.PixelMap[map[x, y]].Color;
				@char = Configuration.PixelMap[map[x, y]].Char;

				Console.ForegroundColor = color;
				Console.Write(@char);
				Console.ResetColor();
				Console.Write("|");

			}
			Console.WriteLine();
		}
	}

	private static void DrawLetters(int[,] map)
	{
		Console.BackgroundColor = ConsoleColor.DarkBlue;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write("   |");
		for (int x = 0; x < map.GetLength(0); x++)
		{
			Console.Write((char) (x + 65) + "|");
		}

		Console.WriteLine();
		Console.ResetColor();
	}

	private static void DrawNumber(int x)
	{
		Console.BackgroundColor = ConsoleColor.DarkBlue;
		Console.ForegroundColor = ConsoleColor.White;
		// if x > 9, draw 1 whitespace, else draw 2 whitespaces
		if (x + 1 > 9)
		{
			Console.Write(x + 1 + " ");
		}
		else
		{
			Console.Write(x + 1 + "  ");
		}

		Console.ResetColor();
	}


	private Configuration.Ships GetShipType(IntegerVector2 coords, int[,] map)
	{
		int x = (int)coords.X;
		int y = (int)coords.Y;
		
		int shipType = map[x, y];
		switch (shipType)
		{
			case 1:
				return Configuration.Ships.Small;
			case 2:
				return Configuration.Ships.Medium;
			case 3:
				return Configuration.Ships.Large;
			case 4:
				return Configuration.Ships.Huge;
			default:
				return Configuration.Ships.None;
		}
	}

	private int GetShipSize(IntegerVector2 coords, int[,] map)
	{
		if (map[coords.X, coords.Y] > 0 && map[coords.X, coords.Y] < 5)
		{
			return map[coords.X, coords.Y];
		}
		return 0;

	}

	private void HitShip(IntegerVector2 target, int[,] defenseMap, int[,] attackMap)
	{
		int x = target.X;
		int y = target.Y;
		
		// check if we hit a ship, if so, mark it as hit (5), if not, mark it as miss (6)
		if (defenseMap[x, y] > 0 && defenseMap[x, y] < 5)
		{
			// if we destroyed a ship, mark all surrounding tiles as hit (6)
			IntegerVector2[] fullShip = GetAllShipFromCoords(target, defenseMap);
			
			if (IsShipDestroyed(fullShip, defenseMap))
			{
				// mark all surrounding tiles as hit (6)
				MarkShipAsDestroyed(attackMap, fullShip);
			}
			else
			{
				// if the ship is not destroyed, mark only the hit cell as hit (5)
				attackMap[x, y] = 5;
			}
			defenseMap[x,y] = 5;
			attackMap[x,y] = 5;
		}
		else
		{
			// mark the cell as miss (6)
			attackMap[x, y] = 6;
		}
	}

	private void MarkShipAsDestroyed(int[,] attackMap, IntegerVector2[] ship)
	{
		// mark all surrounding tiles as hit (6)
		foreach (IntegerVector2 coords in ship)
		{
			int x = coords.X;
			int y = coords.Y;
			
			// mark all surrounding tiles as hit (6)
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if (x + i >= 0 && x + i < attackMap.GetLength(0) && y + j >= 0 && y + j < attackMap.GetLength(1))
					{
						attackMap[x + i, y + j] = 6;
					}
				}
			}
		}

	}

	private IntegerVector2[] GetAllShipFromCoords(IntegerVector2 coords, int[,] map)
	{
		IntegerVector2[] shipCoords = new IntegerVector2[GetShipSize(coords, map)];
		// get ship size
		int shipSize = GetShipSize(coords, map);
		int x = (int)coords.X;
		int y = (int)coords.Y;
		
		// get ship start and end coordinates
		IntegerVector2 start = new(x, y);
		IntegerVector2 dir = new(0, 0);
		
		IntegerVector2[] directions = new IntegerVector2[]
		{
			new IntegerVector2(1, 0),
			new IntegerVector2(0, 1),
			new IntegerVector2(-1, 0),
			new IntegerVector2(0, -1)
		};
		
		// check all directions, until we get to not ship tile,
		// then we know the start of the ship
		foreach (IntegerVector2 direction in directions)
		{
			IntegerVector2 current = coords;
			while (GetShipType(current, map) == GetShipType(coords, map))
			{
				start = current;
				current += direction;
			}
		}
		
		// check all directions, until we get to not ship tile,
		// then we know the end of the ship
		foreach (IntegerVector2 direction in directions)
		{
			IntegerVector2 current = coords;
			while (GetShipType(current, map) == GetShipType(coords, map))
			{
				dir = current;
				current += direction;
			}
		}
		
		// get all cells between start and end (end will be the last cell before we get to not ship tile)
		IntegerVector2[] shipCells = new IntegerVector2[shipSize];
		int i = 0;
		while (start != dir)
		{
			shipCells[i] = start;
			start += new IntegerVector2(1, 0);
			i++;
		}
		
		return shipCells;
	}
	
	private bool IsShipDestroyed(IntegerVector2[] coords, int[,] defenseMap)
	{
		bool isDestroyed = false;
		foreach (IntegerVector2 cell in coords)
		{
			isDestroyed = defenseMap[cell.X, cell.Y] == 5;
		}

		if (coords.Length == 1)
		{
			return true;
		}

		// if we got here, the ship is destroyed
		return isDestroyed;
	}
}

public struct IntegerVector2
{
	public int X;
	public int Y;

	public IntegerVector2(int x, int y)
	{
		X = x;
		Y = y;
	}
	
	public static bool operator ==(IntegerVector2 a, IntegerVector2 b)
	{
		return a.X == b.X && a.Y == b.Y;
	}
	
	public static bool operator !=(IntegerVector2 a, IntegerVector2 b)
	{
		return !(a == b);
	}
	
	public static IntegerVector2 operator +(IntegerVector2 a, IntegerVector2 b)
	{
		return new IntegerVector2(a.X + b.X, a.Y + b.Y);
	}
}