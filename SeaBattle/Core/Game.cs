using SeaBattle.Players;
using SeaBattle.Types;

namespace SeaBattle.Core;

public class Game
{
	public static List<Player> players;
	private Queue<Player> playersQueue;
	
	public void Start()
	{
		Menu menu = new Menu();
		menu.OpenMenu();
		
		players = new List<Player> (menu.GetPlayers ());
		playersQueue = new Queue<Player>(players);
		
		Console.Clear();
		
		// render first player's map
		while (CanGameRun ())
		{
			Player attacker = playersQueue.Dequeue();
			playersQueue.Enqueue(attacker);
			
			Player defender = playersQueue.Peek();
			
			TurnText (attacker, defender);

			Turn(attacker,defender);
			
			Console.Clear();
		}
	}

	private static void TurnText(Player attacker, Player defender)
	{
		if (attacker.IsBot () && defender.IsBot ())
		{
			Console.WriteLine("Bot {0}'s turn, he will be attack {1}", attacker.GetName (),
				defender.GetName ());
			attacker.DefenseMap.RenderMap ();
			attacker.AttackMap.RenderMap ();
			Thread.Sleep(1000);
		}
		else
		{
			Console.WriteLine("Player {0}'s turn, you will be attack {1}", attacker.GetName (),
				defender.GetName ());
			Console.WriteLine("Press any key to continue.");
			Console.ReadKey ();
		}
	}
	

	public void Turn(Player attacker, Player defender)
	{
		IntegerVector2 target = attacker.GetTarget(attacker, defender);
		attacker.Attack(defender, target);
	}
	
	bool CanGameRun()
	{
		var hasShips = GetAllPlayersThatHasShips ();

		// if there is only one player with ships left, he has won the game
		if (hasShips.Count == 1)
		{
			Console.WriteLine("Player " + hasShips[0].GetName () + " has won the game!");
			return false;
		}

		return true;
	}

	private static List<Player> GetAllPlayersThatHasShips()
	{
		List<Player> hasShips = new List<Player> ();
		hasShips.Clear ();
		// check all players
		foreach(Player player in players)
		{
			// find the player who has at least one ship left
			if (player.DefenseMap.HasShips ())
			{
				hasShips.Add(player);
			}
		}

		return hasShips;
	}
}