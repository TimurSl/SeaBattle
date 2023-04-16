namespace SeaBattle;

public class Map
{
	public Cell[,] Grid;
	public IntegerVector2 cursorPosition;
	bool showCursor;
	
	public Map(bool generate = true, bool showCursor = false)
	{
		Grid = generate ? LevelGenerator.GenerateLevel() : LevelGenerator.MakeEmptyMap ();
		this.showCursor = showCursor;
	}
	
	private static void DrawLetters(Cell[,] map)
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

				if (Grid[x, y].IsShip ())
				{
					Ship ship = (Ship) Grid[x, y];

					if (!ship.IsAlive ())
					{
						OutlineShip(ship);
					}
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
}
public enum MapType
{
	Defense,
	Attack,
}