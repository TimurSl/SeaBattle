namespace SeaBattle;

public class Menu
{
	private int playersCount;
	
	public void OpenMenu()
	{
		WelcomeMessage ();
	}

	private void SelectPlayersCount()
	{
		Console.WriteLine("Select players count: ");
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

}