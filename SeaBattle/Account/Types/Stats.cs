namespace SeaBattle.Account.Providers;

public struct Stats
{
	public int Wins { get; set; }
	public int MMR { get; set; }
	
	public Stats(int wins)
	{
		Wins = wins;
	}
}