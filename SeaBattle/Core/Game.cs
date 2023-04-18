using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;
using SeaBattle.Players;

namespace SeaBattle;


public class Game
{
	static List<IPlayer> players = new List<IPlayer> ()
	{
		new HumanPlayer("Player 1"),
		new Bot("Bot 1"),
	};
	private Queue<IPlayer> playersQueue = new Queue<IPlayer>(players);
	

	public void Start()
	{
		WelcomeMessage();

		Console.ReadKey();
		Console.Clear();
		
		// render first player's map
		while (CanGameRun ())
		{
			IPlayer attacker = playersQueue.Dequeue();
			playersQueue.Enqueue(attacker);
			
			IPlayer defender = playersQueue.Peek();
			
			if (attacker.GetType () != typeof(Bot))
			{
				Console.WriteLine("Player {0}'s turn, you will be attack {1}", attacker.GetName (),
					defender.GetName ());
				Console.WriteLine("Press any key to continue.");
				Console.ReadKey ();
			}

			Turn(attacker,defender);
			
			Console.Clear();
		}
	}

	public void Turn(IPlayer attacker, IPlayer defender)
	{
		IntegerVector2 target = attacker.GetTarget(defender.GetAttackMap().Grid);
		attacker.Attack(defender, target);
	}

	private static void WelcomeMessage()
	{
		Console.WriteLine(Figgle.FiggleFonts.Doom.Render("Sea Battle"));

		Console.WriteLine("Welcome to Sea Battle!");
		Console.WriteLine("You can skip your turn by pressing enter.");
		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("S - small ship");
		Console.WriteLine("M - medium ship");
		Console.WriteLine("L - large ship");
		Console.WriteLine("H - huge ship");
		Console.WriteLine();
		Console.WriteLine("X - hit");
		Console.WriteLine("x - miss");
		Console.WriteLine();
		Console.ResetColor();

		Console.WriteLine("Press any key to start the game.");
	}


	bool CanGameRun()
	{
		List<IPlayer> hasShips = new List<IPlayer>();
		hasShips.Clear();
		// check all players
		foreach (IPlayer player in players)
		{
			// find the player who has at least one ship left
			if (player.GetDefenseMap().HasShips ())
			{
				hasShips.Add(player);
			}
		}
		
		// if there is only one player with ships left, he has won the game
		if (hasShips.Count == 1)
		{
			Console.WriteLine("Player " + hasShips[0].GetName () + " has won the game!");
			return false;
		}

		return true;
	}
}