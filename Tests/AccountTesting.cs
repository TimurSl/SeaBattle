using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using SeaBattle.Account.Providers;

namespace Tests;

public class AccountTesting
{
	private XMLProvider provider = new XMLProvider();
	
	private Account ac;
	private (string, string) logPass = ("usr", "pass");
	
	[SetUp]
	public void Setup()
	{
		ac = Account.GetAccount(provider, logPass.Item1, logPass.Item2);
	}

	[Test(Description = "Проверка на авторизацию")]
	public void GetAccount()
	{
		TestContext.WriteLine($"Login: {ac.Login}, Password: {ac.Password}");
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