using System.Diagnostics;
using static SeaBattle.Configuration;

namespace SeaBattle;

public class LevelGenerator
{
	public static Cell[,] GenerateLevel()
	{
		int seed = Math.Abs(Guid.NewGuid().GetHashCode());
		Console.WriteLine("Seed: " + seed);
		Random random = new Random(seed);

		Cell[,] level = MakeEmptyMap();

		PlaceShips(ref level, random);

		return level;
	}

	public static Cell[,] MakeEmptyMap()
	{
		Cell[,] level = new Cell[size, size];
		// Initialize the level with empty cells
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				level[x, y] = new Cell(new IntegerVector2(x, y), CellType.Nothing);
			}
		}

		return level;
	}

	public static void PlaceShips(ref Cell[,] map, Random random)
	{
		var ships = Enum.GetValues(typeof(ShipType)).Cast<ShipType> ();
		foreach (var ship in ships)
		{
			ShipConfiguration shipConfiguration = GetShipConfiguration(ship);
			for (int i = 0; i < shipConfiguration.count; i++)
			{
				PlaceShip(ref map, ship, random, shipConfiguration.length);
			}
		}
	}

	private static ShipConfiguration GetShipConfiguration(ShipType cellType)
	{
		return ShipConfigurations[cellType];
	}

	private static void PlaceShip(ref Cell[,] map, ShipType ship, Random random, int length = 1)
	{
		int x, y;
		bool isPlaced = false;

		while (!isPlaced)
		{
			// Generate a random starting position for the ship
			x = random.Next(map.GetLength(0));
			y = random.Next(map.GetLength(1));

			// Generate a random orientation for the ship (0 = horizontal, 1 = vertical)
			int orientation = random.Next(2);

			// Check if the ship can be placed in the selected orientation without going out of bounds
			if ((orientation == 0 && x + length <= map.GetLength(0)) ||
			    (orientation == 1 && y + length <= map.GetLength(1)))
			{
				// Check if the ship is too close to any existing ships
				bool overlaps = false;

				for (int i = -1; i <= length; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						int cx = x + (orientation == 0 ? i : 0);
						int cy = y + (orientation == 1 ? i : 0);

						// Check if the current cell is already occupied by another ship, or is adjacent to another ship
						for (int ii = -1; ii <= 1; ii++)
						{
							for (int jj = -1; jj <= 1; jj++)
							{
								int tx = cx + ii;
								int ty = cy + jj;

								if (tx >= 0 && tx < map.GetLength(0) && // check if X is in bounds
								    ty >= 0 && ty < map.GetLength(1) && // check if Y is in bounds
								    map[tx, ty].CellType != CellType.Nothing) // check if the cell is occupied
								{
									overlaps = true;
									break;
								}
							}

							if (overlaps) break;
						}

						if (overlaps) break;
					}

					if (overlaps) break;
				}

				// If the ship doesn't overlap with any existing ships, place it on the map
				if (!overlaps)
				{
					Cell[] shipCells = new Cell[length];
					for (int i = 0; i < length; i++)
					{
						int cx = x + (orientation == 0 ? i : 0);
						int cy = y + (orientation == 1 ? i : 0);
						Ship shipObj = new Ship(new IntegerVector2(cx, cy), ship);
						shipCells[i] = shipObj;
						map[cx, cy] = shipObj;
					}
					// add all cells to all cells
					foreach (var cell1 in shipCells)
					{
						var cell = (Ship) cell1;
						cell.ShipCells = shipCells;
					}

					isPlaced = true;
				}
			}
		}
	}
}