using System.Numerics;
using System.Text.RegularExpressions;
using SeaBattle.Players;

namespace SeaBattle;


public class Game
{

	bool isPlayer1Turn = true;
	
	private IPlayer player1 = new Player();
	private IPlayer player2 = new Bot();
	

	public Game()
	{
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
				Turn(player1, player2MapDefense, player1MapAttack);
				
				isPlayer1Turn = false;
			}
			// Otherwise, the enemy takes their turn
			else
			{
				Turn(player2, player1MapDefense, player2MapAttack);

				isPlayer1Turn = true;
			}

			// Check if all ships are destroyed
			// ...

			// Display the game board
			Console.Clear();
		}

	}

	public void Turn(IPlayer player, int[,] victimMap, int[,] attackerMap)
	{
		IntegerVector2 target = player.GetTarget(victimMap);

		if (target != new IntegerVector2(-1, -1))
		{
			HitShip(target, victimMap, attackerMap);
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
		Console.WriteLine("H - huge ship");
		Console.WriteLine();
		Console.WriteLine("X - hit");
		Console.WriteLine("x - miss");
		Console.WriteLine();
		Console.ResetColor();

		Console.WriteLine("Press any key to start the game.");
	}


	bool CanGameRun()
	{
		bool player1HasShips = player1MapDefense.Cast<int>().Any(x => x == 1 || x == 2 || x == 3 || x == 4);
		bool player2HasShips = player2MapDefense.Cast<int>().Any(x => x == 1 || x == 2 || x == 3 || x == 4);
		
		if (player1HasShips && player2HasShips)
		{
			return true;
		}

		if (player1HasShips)
		{
			Console.WriteLine("Player 1 wins!");
		}
		else
		{
			Console.WriteLine("Player 2 wins!");
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

	

	private int GetShipSize(IntegerVector2 coords, int[,] map)
	{
		if (map[coords.X, coords.Y] > 0)
		{
			return map[coords.X, coords.Y];
		}
		return 0;

	}

	private void HitShip(IntegerVector2 target, int[,] defenseMap, int[,] attackMap)
	{
		int x = target.X;
		int y = target.Y;

		if (IsHit(target, defenseMap))
		{
			bool isShipDestroyed = IsShipDestroyed(target, defenseMap, out var shipCells);
			if (isShipDestroyed)
			{
				OutlineShip(shipCells, ref attackMap, ref defenseMap);
			}
			
			attackMap[x, y] = (int) Configuration.Ships.Hit;
			defenseMap[x, y] = (int) Configuration.Ships.Hit;
		}
		else
		{
			// mark cell as miss
			attackMap[x, y] = (int) Configuration.Ships.Miss;
		}
		
	}

	private void OutlineShip(IntegerVector2[] shipCells, ref int[,] attackerMap, ref int[,] victimMap)
	{
		foreach (var cell in shipCells)
		{
			OutlineCell(cell, ref attackerMap);
		}
		
		foreach (var cell in shipCells)
		{
			attackerMap[cell.X, cell.Y] = (int) Configuration.Ships.Hit;
			victimMap[cell.X, cell.Y] = (int) Configuration.Ships.Hit;
		}
	}

	private void OutlineCell(IntegerVector2 cell, ref int[,] map)
	{
		int x = cell.X;
		int y = cell.Y;
		
		// mark cell as hit
		
		// mark all cells around as hit and also check if our coords are out of bounds (if they are, skip them)
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				if (x + i >= 0 && x + i < map.GetLength(0) && y + j >= 0 && y + j < map.GetLength(1))
				{
					map[x + i, y + j] = (int) Configuration.Ships.Miss;
				}
			}
		}

		map[x, y] = (int) Configuration.Ships.Hit;

	}

	private bool IsHit(IntegerVector2 point, int[,] map)
	{
		return map[point.X, point.Y] > 0;
	}
	
	
	private bool IsShipDestroyed(IntegerVector2 coords, int[,] defenseMap, out IntegerVector2[] shipCells)
	{
		int x = (int)coords.X;
		int y = (int)coords.Y;

		// Check if the hit coordinate contains a ship.
		if (defenseMap[x, y] == 0)
		{
			shipCells = new IntegerVector2[0];
			return false;
		}

		int shipSize = GetShipSize(coords, defenseMap);
		Console.WriteLine("Ship size: " + shipSize);
		
		if (shipSize == 0)
		{
			shipCells = new IntegerVector2[0];
			return false;
		}

		//TODO: make check if ship is destroyed
		
		shipCells = new IntegerVector2[0];
		return false;
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
	
	public static IntegerVector2 operator +(IntegerVector2 a, int b)
	{
		return new IntegerVector2(a.X + b, a.Y + b);
	}
	
	public static IntegerVector2 operator *(IntegerVector2 a, int b)
	{
		return new IntegerVector2(a.X * b, a.Y * b);
	}
}