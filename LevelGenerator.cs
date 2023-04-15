namespace SeaBattle;

public class LevelGenerator {
    
    public WaterObject[,] GenerateLevel(int seed) 
    {
        Random random = new Random(seed);
        WaterObject[,] level = new WaterObject[Configuration.size, Configuration.size];

        // fill all level with air
        for (int x = 0; x < Configuration.size; x++)
        {
            for (int y = 0; y < Configuration.size; y++)
            {
                level[x, y] = new Air(x, y);
            }
        }
        
        // generate ships
        foreach (ShipTypes types in Configuration.ShipArray)
        {
            int count = 0;
            switch (types)
            {
                case ShipTypes.SmallShip:
                    count = Configuration.maxSmallShips;
                    break;
                case ShipTypes.MediumShip:
                    count = Configuration.maxMediumShips;
                    break;
                case ShipTypes.LargeShip:
                    count = Configuration.maxLargeShips;
                    break;
                case ShipTypes.HugeShip:
                    count = Configuration.maxHugeShips;
                    break;
            }
            
            for (int i = 0; i < count; i++)
            {
                int x = random.Next(Configuration.size);
                int y = random.Next(Configuration.size);
                while (level[x, y].type != WaterObjectTypes.Air)
                {
                    x = random.Next(Configuration.size);
                    y = random.Next(Configuration.size);
                }
                bool isHorizontal = random.Next(2) == 0;
                
                level[x, y] = new Ship(x, y, (int) types, isHorizontal, types);
            }
        }
        
        return level;
    }
    
}