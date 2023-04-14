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
		Configuration.seed = 0;

		// Generate the two maps for the players
		LevelGenerator levelGenerator = new LevelGenerator();
		player1MapDefense = levelGenerator.GenerateLevel(Configuration.seed + 1);
		player2MapDefense = levelGenerator.GenerateLevel(Configuration.seed - 1);

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
		int shipSize = 0;
		if (map[coords.X, coords.Y] < 1 && map[coords.X, coords.Y] > 5)
		{
			return shipSize;
		}
		else
		{
			return 0;
		}
	}

	private void HitShip(IntegerVector2 target, int[,] defenseMap, int[,] attackMap)
	{
		int x = target.X;
		int y = target.Y;
		
		// check if we hit a ship, if so, mark it as hit (5), if not, mark it as miss (6)
		if (GetShipType(target, defenseMap) != Configuration.Ships.None)
		{
			defenseMap[x,y] = 5;
			attackMap[x,y] = 5;
			// if we destroyed a ship, mark all surrounding tiles as hit (6)
			if (IsShipDestroyed(new IntegerVector2(x, y), defenseMap))
			{
				// mark all surrounding tiles as hit (6)
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						int xx = x + i;
						int yy = y + j;
						if (xx >= 0 && xx < defenseMap.GetLength(0) && yy >= 0 && yy < defenseMap.GetLength(1))
						{
							if (defenseMap[xx, yy] != 5)
							{
								defenseMap[xx, yy] = 6;
								attackMap[xx, yy] = 6;
							}
						}
					}
				}
			}
			else
			{
				// if the ship is not destroyed, mark only the hit cell as hit (5)
				attackMap[x, y] = 5;
			}
		}
		else
		{
			// mark the cell as miss (6)
			attackMap[x, y] = 6;
		}
	}
	private bool IsShipDestroyed(IntegerVector2 coords, int[,] defenseMap)
	{
		int shipSize = GetShipSize(coords, defenseMap);
		int x = (int)coords.X;
		int y = (int)coords.Y;

		IntegerVector2 startShipPosition = new IntegerVector2(x, y);
		IntegerVector2 direction = new IntegerVector2(0, 0);
		
		// get ship start position and direction
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				int xx = x + i;
				int yy = y + j;
				if (xx >= 0 && xx < defenseMap.GetLength(0) && yy >= 0 && yy < defenseMap.GetLength(1))
				{
					if (defenseMap[xx, yy] == shipSize)
					{
						startShipPosition = new IntegerVector2(xx, yy);
						direction = new IntegerVector2(i, j);
					}
				}
			}
		}

		bool isDestroyed = false;
		// check if the ship is destroyed
		for (int i = 0; i < shipSize; i++)
		{
			int xx = startShipPosition.X + i * direction.X;
			int yy = startShipPosition.Y + i * direction.Y;
			if (defenseMap[xx, yy] == 5)
			{
				isDestroyed = true;
			}
			else
			{
				isDestroyed = false;
				break;
			}
		}
		
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
}