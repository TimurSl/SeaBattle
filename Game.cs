using System.Numerics;
using System.Text.RegularExpressions;

namespace SeaBattle;


public class Game
{
	int[,] player1MapDefense;
	int[,] player1MapAttack;
	
	int[,] player2MapDefense;
	int[,] player2MapAttack;
	
	bool isPlayer1Turn = true;
	

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
		// Welcome the user and prompt them to start the game
		Console.WriteLine("Welcome to Sea Battle!");
		Console.WriteLine("Press any key to start the game.");
		Console.WriteLine("You can skip your turn by pressing enter.");
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
				PlayerTurn();
				isPlayer1Turn = false;
			}
			// Otherwise, the enemy takes their turn
			else
			{
				EnemyTurn();
				isPlayer1Turn = true;
			}
			Console.Clear();
		}
	}

	private void EnemyTurn()
	{
		// pick random position what is not miss, if hit, pick random position around it
		Random random = new Random();
		int x = random.Next(Configuration.size);
		int y = random.Next(Configuration.size);
		while (player1MapDefense[x, y] == 6 || player1MapDefense[x, y] == 5)
		{
			x = random.Next(Configuration.size);
			y = random.Next(Configuration.size);
		}
		
		if (player1MapDefense[x, y] == 5)
		{
			// hit, pick random position around it
			int direction = random.Next(4);
			switch (direction)
			{
				case 0:
					x++;
					break;
				case 1:
					x--;
					break;
				case 2:
					y++;
					break;
				case 3:
					y--;
					break;
			}
		}
		
		// check if we hit a ship, if so, mark it as hit (5), if not, mark it as miss (6)
		if (GetShipType(new Vector2(x, y), player1MapDefense) != Configuration.Ships.None)
		{
			player1MapDefense[x, y] = 5;
			player2MapAttack[x, y] = 5;
		}
		else
		{
			player1MapDefense[x, y] = 6;
			player2MapAttack[x, y] = 6;
		}
	}

	private void PlayerTurn()
	{
		Vector2 input = ReadInput();

		if (input.X == -1 && input.Y == -1 || input.X > Configuration.size || input.Y > Configuration.size)
		{
			Console.WriteLine("Invalid input");
		}
		else
		{
			int x = (int) input.X;
			int y = (int) input.Y;

			CheckHitOrMiss(x, y);
		}

		Console.Clear();
	}

	private void CheckHitOrMiss(int x, int y)
	{
		if (GetShipType(new Vector2(x, y), player2MapDefense) != Configuration.Ships.None)
		{
			HitShip(x, y, player2MapDefense, player1MapAttack);
		}
		else
		{
			player2MapDefense[x, y] = 6;
			player1MapAttack[x, y] = 6;
		}
	}

	private Vector2 ReadInput()
	{
		Console.Write("Enter coordinates to attack: ");
		string input = Console.ReadLine();
		string pattern = @"^([A-Za-z]+)(\d+)$"; // pattern to match one or more letters followed by one or more digits
		Match match = Regex.Match(input, pattern);
		if (!match.Success)
		{
			return new Vector2(-1, -1);
		}
		else
		{
			string column = match.Groups[1].Value.ToUpper();
			int row = int.Parse(match.Groups[2].Value) - 1;
			int columnLength = column.Length;
			int columnNumber = 0;
			for (int i = 0; i < columnLength; i++)
			{
				columnNumber += (column[i] - 'A' + 1) * (int)Math.Pow(26, columnLength - i - 1);
			}
			return new Vector2(row, --columnNumber);
		}
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
		Console.BackgroundColor = ConsoleColor.DarkBlue;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write("   |");
		for (int x = 0; x < map.GetLength(0); x++)
		{
			Console.Write((char)(x + 65) + "|");
		}
		Console.WriteLine();
		Console.ResetColor();
		
		for (int x = 0; x < map.GetLength(0); x++)
		{
			Console.BackgroundColor = ConsoleColor.DarkBlue;
			Console.ForegroundColor = ConsoleColor.White;
			// if x > 9, draw 1 whitespace, else draw 2 whitespaces
			if (x+1 > 9)
			{
				Console.Write(x+1 + " ");
			}
			else
			{
				Console.Write(x+1 + "  ");
			}
			Console.ResetColor();
			
			Console.Write("|");

			// numbers
			for (int y = 0; y < map.GetLength(1); y++)
			{
				ConsoleColor color = ConsoleColor.White;
				string @char = "";
				// draw map
				switch (map[x,y])
				{
					case 0: 
						@char = " ";
						color = ConsoleColor.Black;
						break;
					case 1:
						@char = "S";
						color = ConsoleColor.Yellow;
						break;
					case 2:
						@char = "M";
						color = ConsoleColor.Blue;
						break;
					case 3:
						@char = "L";
						color = ConsoleColor.Green;
						break;
					case 4:
						@char = "H";
						color = ConsoleColor.Magenta;
						break;
					// hit
					case 5:
						@char = "X";
						color = ConsoleColor.Red;
						break;
					// miss
					case 6:
						@char = "x";
						color = ConsoleColor.DarkYellow;
						break;
					default:
						@char = " ";
						color = ConsoleColor.Black;
						break;
				}
				
				Console.ForegroundColor = color;
				Console.Write(@char);
				Console.ResetColor();
				Console.Write("|");

			}
			Console.WriteLine();
		}
	}


	private Configuration.Ships GetShipType(Vector2 coords, int[,] map)
	{
		int x = (int)coords.X;
		int y = (int)coords.Y;
		
		int shipType = map[x, y];
		if (shipType < 1 || shipType > 4)
		{
			return Configuration.Ships.None;
		}
		else
		{
			return (Configuration.Ships)shipType;
		}
	}

	private void HitShip(int x, int y, int[,] defenseMap, int[,] attackMap)
	{
		// check if we hit a ship, if so, mark it as hit (5), if not, mark it as miss (6)
		if (GetShipType(new Vector2(x, y), defenseMap) != Configuration.Ships.None)
		{
			defenseMap[x, y] = 5;
			attackMap[x, y] = 5;
			// if we destroyed a ship, mark all surrounding tiles as hit (6)
			if (IsShipDestroyed(new Vector2(x, y), defenseMap))
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
	private bool IsShipDestroyed(Vector2 coords, int[,] defenseMap)
	{
		int shipSize = (int)GetShipType(coords, defenseMap);
		int x = (int)coords.X;
		int y = (int)coords.Y;

		
		return false;
	}



}