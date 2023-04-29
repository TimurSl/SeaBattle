using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SeaBattle.Account.Providers;

namespace Tests;

public class AccountTesting
{
	private WebAccountProvider provider = new WebAccountProvider();
	
	private Account ac;
	private (string, string) logPass = ("usr", "pass");
	
	[SetUp]
	public void Setup()
	{
		ac = provider.GetAccount(logPass.Item1, logPass.Item2);
	}

	[Test(Description = "Проверка на авторизацию")]
	public void GetAccount()
	{
		Assert.That(ac.Login, Is.EqualTo("usr"));
	}

	[Test]
	public void GiveWins()
	{
		StatsData stats = new StatsData () {Wins = "4", Mmr = "100"};
		
		string json = JsonConvert.SerializeObject(stats);
		
		TestContext.WriteLine(json);

		Account response = provider.ModifyStats(logPass.Item1, logPass.Item2, stats);

		Assert.That(response.Stats.MMR, Is.EqualTo(100));
	}
}