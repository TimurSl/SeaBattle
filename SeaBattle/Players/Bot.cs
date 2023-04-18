using System.Numerics;
using SeaBattle.Types;

namespace SeaBattle.Players;

public class Bot : SeaBattle.IPlayer
{
	public Map DefenseMap;
	public Map AttackMap;

	private string name = "Bot";

	private Random random = new Random();

	
	public Bot(string name = "Bot")
	{
		this.name = name;
		DefenseMap = new Map (useLastHit:true);
		AttackMap = new Map(LevelCreationType.Empty, true);
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