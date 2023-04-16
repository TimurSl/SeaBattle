using System.Numerics;

namespace SeaBattle.Players;

public class Bot : SeaBattle.IPlayer
{
	public Map DefenseMap;
	public Map AttackMap;
	
	private string name = "Bot";
	public int MapSeed { get; set; }

	
	public Bot(string name = "Bot", int mapSeed = 0)
	{
		this.name = name;
		MapSeed = mapSeed;
		DefenseMap = new Map (useLastHit:true);
		AttackMap = new Map(false, true);
	}

	public Map GetDefenseMap()
	{
		return DefenseMap;
	}

	public Map GetAttackMap()
	{
		return AttackMap;
	}

	public IntegerVector2 GetTarget(Cell[,] attackMap)
	{
		// pick random position what is not miss, if hit, pick random position around it
		Random random = new Random();
		int x = random.Next(Configuration.size);
		int y = random.Next(Configuration.size);
		while (attackMap[x,y].IsHitOrMiss ())
		{
			x = random.Next(Configuration.size);
			y = random.Next(Configuration.size);
		}

		return new IntegerVector2(x, y);
	}
	
	public override string ToString()
	{
		return "Bot";
	}
	


	public string GetName()
	{
		return name;
	}
}