using SeaBattle.Core.Types;
using SeaBattle.Players;
using SeaBattle.Settings;
using SeaBattle.Types;

namespace SeaBattle.Core;

public class Game
{
	public static List<Player> players;
	private Queue<Player> playersQueue;
	
	private RoundManager roundManager = new RoundManager();
	private TurnManager turnManager = new();

	public Game(GameLaunchParams @params)
	{
		players = new List<Player>(@params.Players);
		playersQueue = new Queue<Player>(players);
		roundManager.InitializeScores(players);
	}
	
	public void Start()
	{
		Console.Clear();

		// render first player's map
		while (CanGameRun ())
		{ 
			roundManager.PrintRound ();
			
			Player attacker = playersQueue.Dequeue();
			playersQueue.Enqueue(attacker);
			
			Player defender = playersQueue.Peek();
			
			turnManager.TurnText (attacker, defender);

			turnManager.Turn(attacker,defender);
			
			Console.Clear();
		}
	}
	
	bool CanGameRun()
	{
		if (roundManager.IsGameOver ())
		{
			Player winner = roundManager.GetWinner ();
			winner.Account.AddWin ();
			
			Console.Clear ();
			Console.WriteLine("Player " + winner.GetName () + " has won the game!");
			Thread.Sleep(2000);
			return false;
		}
		var hasShips = GetAllPlayersThatHasShips ();

		// if there is only one player with ships left, he has won the game
		if (hasShips.Count == 1)
		{
			if (roundManager.CanContinue ())
			{
				Console.Clear ();
				roundManager.NextRound (hasShips[0]);

				Console.WriteLine($"Player {hasShips[0].GetName ()} has won the round!, he has {roundManager.scores[hasShips[0]]} / {Configuration.roundsToWin} points!");
				Thread.Sleep(2000);
				
				playersQueue.Clear ();
				playersQueue = new Queue<Player>(players);
				turnManager.ResetMaps (players); 
			
				Start ();
			}

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