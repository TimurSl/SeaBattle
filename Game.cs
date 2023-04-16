using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;
using SeaBattle.Players;

namespace SeaBattle;


public class Game
{
	static List<IPlayer> players = new List<IPlayer> ()
	{
		new HumanPlayer("Zenisoft"),
		new Bot(),
	};
	private Queue<IPlayer> playersQueue = new Queue<IPlayer>(players);


	public Game()
	{
		
	}

	public void Start()
	{
		Console.WriteLine(Figgle.FiggleFonts.Doom.Render("Sea Battle"));
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
		
		
		Console.ReadKey();

	}

	public void Turn(IPlayer attacker, IPlayer defender)
	{
		IntegerVector2 target = attacker.GetTarget(defender.GetAttackMap().Grid);
		attacker.Attack(defender, target);
	}

	private static void WelcomeMessage()
	{
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
		// check all players
		foreach (IPlayer player in players)
		{
			// find the player who has at least one ship left
			if (player.GetDefenseMap().HasShips ())
			{
				return true;
			}
		}
		
		// find player who has ships left
		IPlayer playerWithShips = players.Find(player => player.GetDefenseMap().HasShips());
		if (playerWithShips != null)
		{
			Console.WriteLine("Player " + playerWithShips.GetName () + " has won the game!");
			Console.ReadKey ();
			return false;
		}
		
		return false;
	}
}