using SeaBattle.Cells;
using SeaBattle.Settings;
using SeaBattle.Types;

namespace SeaBattle.MapCreators.Types;

public class Map
{
	public Cell[,] Grid;

	public IntegerVector2 cursorPosition;
	public IntegerVector2 lastHit = new IntegerVector2(-1, -1);


	private bool showCursor;
	private bool useLastHit = false;
	
	private ConsoleColor mapBackgroundColor;
	private LevelCreationType levelType;

	public Map(LevelCreationType levelType = LevelCreationType.Random, bool showCursor = false, bool useLastHit = false)
	{
		CreateGrid (levelType);
		
		this.levelType = levelType;
		this.showCursor = showCursor;
		this.useLastHit = useLastHit;
		
		Random r = new Random();
		mapBackgroundColor = (ConsoleColor)r.Next(1,15);
	}

	private void CreateGrid(LevelCreationType levelType)
	{
		Grid = levelType switch
		{
			LevelCreationType.Empty => LevelGenerator.MakeEmptyMap (),
			LevelCreationType.Random => LevelGenerator.GenerateLevel (),
			LevelCreationType.Manual => ManualMapCreator.GetLevel (),
			_ => Grid
		};
	}

	private void DrawLetters(Cell[,] map)
	{
		Console.BackgroundColor = mapBackgroundColor;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write("   |");
		for (int x = 0; x < map.GetLength(0); x++)
		{
			Console.Write((char) (x + 65) + "|");
		}

		Console.WriteLine();
		Console.ResetColor();
	}
	
	
	private void DrawNumber(int x)
	{
		Console.BackgroundColor = mapBackgroundColor;
		Console.ForegroundColor = ConsoleColor.White;
		x++;
		int digits = (int) Math.Floor(Math.Log10(Configuration.size) + 1);
		int xDigits = (int) Math.Floor(Math.Log10(x) + 1);

		int amountOfWhitespaces = (digits - xDigits) + 1;
		
		Console.Write(x);
		for (int i = 0; i < amountOfWhitespaces; i++)
		{
			Console.Write(" ");
		}

		Console.ResetColor();
	}
	
	public void SetCursorPosition(IntegerVector2 position)
	{
		cursorPosition = position;
	}

	public void RenderMap()
	{
		DrawLetters(Grid);
		
		for (int x = 0; x < Grid.GetLength(0); x++)
		{
			DrawNumber(x);

			for (int y = 0; y < Grid.GetLength(1); y++)
			{
				Console.Write("|");

				if (Grid[x, y].CellType == Configuration.CellType.Ship)
				{
					Ship ship = (Ship) Grid[x, y];

					if (!ship.IsAlive ())
					{
						OutlineShip(ship);
					}
				}
				if (useLastHit && new IntegerVector2(x, y) == lastHit)
				{
					Console.BackgroundColor = ConsoleColor.DarkRed;
				}
				
				ConsoleColor color = Grid[x, y].GetCellColor ();
				int cellType = (int) Grid[x, y].CellType;
				
				if (new IntegerVector2(x, y) == cursorPosition && showCursor)
				{
					Console.BackgroundColor = ConsoleColor.DarkGray;
				}
				
				Console.ForegroundColor = color;
				Console.Write(Grid[x, y].GetCellChar ());
				Console.ResetColor();

			}
			Console.Write("|");

			Console.WriteLine();
		}

		Console.WriteLine();
	}

	public void OutlineShip(Ship ship)
	{
		// get all cells around the ship
		Cell[] cells = ship.ShipCells;

		foreach (Cell cell in cells)
		{
			OutlineCell(cell);
		}
	}
	
	private void OutlineCell(Cell cell)
	{
		for (int x = cell.Position.X - 1; x <= cell.Position.X + 1; x++)
		{
			for (int y = cell.Position.Y - 1; y <= cell.Position.Y + 1; y++)
			{
				if (x >= 0 && x < Grid.GetLength(0) && y >= 0 && y < Grid.GetLength(1))
				{
					if (Grid[x, y].CellType == Configuration.CellType.Nothing)
					{
						Grid[x, y].CellType = Configuration.CellType.Miss;
					}
				}
			}
		}
	}
	
	public bool HasShips()
	{
		for (int x = 0; x < Grid.GetLength(0); x++)
		{
			for (int y = 0; y < Grid.GetLength(1); y++)
			{
				if (Grid[x, y].CellType == Configuration.CellType.Ship)
				{
					return true;
				}
			}
		}

		return false;
	}

	public void ResetMap()
	{
		// regenerate the map
		CreateGrid(levelType);
	}
}