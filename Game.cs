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
		
		char[,] board = new char[10, 10]; // Create a 10x10 game board
// Initialize all cells to be water
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				board[i, j] = '~';
			}
		}

// Place ships on the board
// ...

// Game loop
		while (true) {
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					Console.Write(board[i, j] + " ");
				}
				Console.WriteLine();
			}
			// Take user input
			Console.Write("Enter a coordinate (e.g. A1): ");
			string input = Console.ReadLine();
			// Validate and convert user input
			int row = int.Parse(input.Substring(1)) - 1;
			int col = input[0] - 'A';

			// Check if coordinate hit a ship
			if (board[row, col] == 'S') {
				Console.WriteLine("Hit!");
				board[row, col] = 'X';
				// Check if ship is destroyed
				// ...
			} else {
				Console.WriteLine("Miss!");
				board[row, col] = 'O';
			}

			// Check if all ships are destroyed
			// ...

			// Display the game board
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