using SeaBattle.Players;
using SeaBattle.Players.Inputs;
using SeaBattle.Players.Inputs.Bot;
using SeaBattle.Players.Interfaces;
using SeaBattle.Players.Types;
using SeaBattle.Settings;

namespace SeaBattle.Core;

public class Menu
{
	private List<Player> players = new List<Player>();

	public void OpenMenu()
	{
		WelcomeMessage ();
		PlayerSetup ();
		Game.players = players;
	}
	
	public List<Player> GetPlayers()
	{
		return players;
	}

	private void PlayerSetup()
	{
		int playersCount = 2;

		for (int i = 0; i < playersCount; i++)
		{
			AskForPlayer (i);
		}
	}

	private void AskForPlayer(int number)
	{
		Player player;
		Console.WriteLine($"Select player type (Player {number}):");
		foreach (PlayerType type in Enum.GetValues(typeof(PlayerType)))
		{
			Console.WriteLine($"{(int) type} - {type}");
		}
		Console.Write("Player type (default: 1): ");
		string playerT = Console.ReadLine () ?? "1";
		if (playerT == "")
			playerT = "1";
		
		PlayerType playerType = (PlayerType) int.Parse(playerT);
		
		Console.Write("Player name: ");
		string playerName = Console.ReadLine () ?? "Player " + (number + 1);
		if (playerType == PlayerType.Bot)
		{
			// select bot difficulty
			Console.WriteLine("Select bot difficulty:");
			foreach (BotDifficulties diff in Enum.GetValues(typeof(BotDifficulties)))
			{
				Console.WriteLine($"{(int) diff} - {diff}");
			}
			Console.Write("Bot difficulty (default: 1): ");
			string difficulty = Console.ReadLine () ?? "1";
			if (difficulty == "")
				difficulty = "1";
			
			BotDifficulties botDifficulty = (BotDifficulties) int.Parse(difficulty);
			IInput input = null;
			switch (botDifficulty)
			{
				case BotDifficulties.PatrickStar:
					input = new EasyBotInput ();
					break;
				case BotDifficulties.Honest:
					input = new MediumBotInput ();
					break;
				case BotDifficulties.Hard:
					input = new HardBotInput ();
					break;
				default:
					input = new EasyBotInput ();
					break;
			}
			player = new Player(new PlayerParams () { Type = PlayerType.Bot, Name = playerName, Input = input });
		}
		else
		{
			PlayerArrowInput input = new PlayerArrowInput();
			player = new Player(new PlayerParams () { Type = PlayerType.Human, Name = playerName, Input = input });
		}
		players.Add(player);
		Console.Clear();
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
		
	}

}