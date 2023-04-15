namespace SeaBattle;

public class LevelGenerator {
    
    public int[, ] GenerateLevel(int seed) 
    {
        Random random = new Random(seed);
        int[,] level = new int[Configuration.size, Configuration.size];

        PlaceShips(ref level, random);

        return level;
    }

    public void PlaceShips(ref int[, ] map, Random random) 
    {
        foreach(var ship in Configuration.ShipArray) {
            int shipCount = GetShipCount(ship);
            for (int i = 0; i < shipCount; i++) {
                PlaceShip(ref map, ship, random);
            }
        }
    }

    private int GetShipCount(Configuration.Ships ship) {
        switch (ship) {
        case Configuration.Ships.Small:
            return Configuration.maxSmallShips;
        case Configuration.Ships.Medium:
            return Configuration.maxMediumShips;
        case Configuration.Ships.Large:
            return Configuration.maxLargeShips;
        case Configuration.Ships.Huge:
            return Configuration.maxHugeShips;
        }
        return 0;
    }

    private void PlaceShip(ref int[,] map, Configuration.Ships ship, Random random)
    {
        int x, y;
        bool isPlaced = false;
        int length = (int)ship;

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

                                if (tx >= 0 && tx < map.GetLength(0) && ty >= 0 && ty < map.GetLength(1) && map[tx, ty] != 0)
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
                    for (int i = 0; i < length; i++)
                    {
                        int cx = x + (orientation == 0 ? i : 0);
                        int cy = y + (orientation == 1 ? i : 0);
                        map[cx, cy] = ship.GetHashCode();
                    }

                    isPlaced = true;
                }
            }
        }
    }
}