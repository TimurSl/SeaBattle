using System.Numerics;

namespace SeaBattle.Players;

public class Bot : IPlayer
{

	public IntegerVector2 GetTarget(int[,] attackMap)
	{
		// pick random position what is not miss, if hit, pick random position around it
		Random random = new Random();
		int x = random.Next(Configuration.size);
		int y = random.Next(Configuration.size);
		while (attackMap[x, y] == 6 || attackMap[x, y] == 5)
		{
			x = random.Next(Configuration.size);
			y = random.Next(Configuration.size);
		}
		
		if (attackMap[x, y] == 5)
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
		
		return new IntegerVector2(x, y);
	}
}