using SeaBattle.Settings;

namespace SeaBattle.Core;

public class RoundManager
{
	private int currentRound = 1;

	public void NextRound()
	{
		currentRound++;
	}
	
	public bool CanContinue()
	{
		return currentRound < Configuration.rounds;
	}
	
	public void PrintRound()
	{
		Console.WriteLine($"Round {currentRound} of {Configuration.rounds}");
	}
}