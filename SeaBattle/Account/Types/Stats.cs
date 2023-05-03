namespace SeaBattle.Account.Providers;

[Serializable]
public struct Stats
{
	public int Wins { get; set; }
	public int MMR { get; set; }
	
	public Stats(int wins)
	{
		Wins = wins;
	}
	
	public Stats(int wins, int mmr)
	{
		Wins = wins;
	}
	
	public StatsData ToStatsData()
	{
		StatsData statsData = new StatsData();
		statsData.Wins = Wins.ToString();
		statsData.Mmr = MMR.ToString();
		return statsData;
	}
}