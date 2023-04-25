using SeaBattle.Players;
using SeaBattle.Types;

namespace SeaBattle.Core;

public class TurnManager
{
	public void ResetMaps(List<Player> players)
	{
		foreach(Player player in players)
		{
			player.DefenseMap.ResetMap ();
			player.AttackMap.ResetMap ();
		}
	}

	public void TurnText(Player attacker, Player defender)
	{
		if (attacker.IsBot () && defender.IsBot ())
		{
			Console.WriteLine("Bot {0}'s turn, he will be attack {1}", attacker.GetName (),
				defender.GetName ());
			attacker.DefenseMap.RenderMap ();
			attacker.AttackMap.RenderMap ();
			Thread.Sleep(1000);
		}

		if (!attacker.IsBot () && !defender.IsBot ())
		{
			Console.WriteLine("Player {0}'s turn, you will be attack {1}", attacker.GetName (), defender.GetName ());
			Console.WriteLine("Press any key to continue.");
			Console.ReadKey ();
		}
	}


	public void Turn(Player attacker, Player defender)
	{
		do
		{
			IntegerVector2 target = attacker.GetTarget(attacker, defender);
			attacker.Attack(defender, target);
		} 
		while (attacker.IsStreak);
	}
}