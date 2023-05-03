using System.Net.Http;
using Newtonsoft.Json;

namespace SeaBattle.Account.Providers;

/// <summary>
/// Dont use this yet
/// </summary>
public class WebAccountProvider : IAccountProvider
{
	private readonly string registerUrl = "https://zenisoft.net.ua/seabattle/register.php";
	private readonly string modifyUrl = "https://zenisoft.net.ua/seabattle/modify.php";
	
	private static readonly HttpClient client = new HttpClient();
	
	public Account GetAccount(string login, string password)
	{
		var values = new Dictionary<string, string>
		{
			{ "login", login },
			{ "password", password },
		};
		
		
		var response = PostAndGetResponse(registerUrl, values).Result;
		
		if (response == "{\"success\":false}" || string.IsNullOrEmpty(response))
		{
			return new Account($"ERROR: {response}", "", new Stats(0), this);
		}

		AccountData accountData = JsonConvert.DeserializeObject<AccountData>(response);
		StatsData statsData = JsonConvert.DeserializeObject<StatsData>(accountData.Stats);
		
		Stats stats = new Stats(int.Parse(statsData.Wins)) {MMR = int.Parse(statsData.Mmr)};
		
		return new Account(accountData.Login, accountData.Password, stats, this);
	}

	public Account ModifyStats(string login, string password, StatsData newStats)
	{
		var values = new Dictionary<string, string>
		{
			{ "login", login },
			{ "password", password },
			{ "new_data", JsonConvert.SerializeObject(newStats) },
		};

		var response = PostAndGetResponse(modifyUrl, values).Result;

		if (response == "{\"success\":false}" || string.IsNullOrEmpty(response))
		{
			return new Account($"ERROR: {response}", "", new Stats(0), this);
		}

		AccountData accountData = JsonConvert.DeserializeObject<AccountData>(response);
		StatsData statsData = JsonConvert.DeserializeObject<StatsData>(accountData.Stats);
		
		Stats stats = new Stats(int.Parse(statsData.Wins)) {MMR = int.Parse(statsData.Mmr)};
		
		return new Account(accountData.Login, accountData.Password, stats, this);
	}

	public string TestModifyStats(string login, string pass, StatsData @new)
	{
		string newData = JsonConvert.SerializeObject(@new);
		var values = new Dictionary<string, string>
		{
			{ "login", login },
			{ "password", pass },
			{ "new_data", newData },
		};

		var response = PostAndGetResponse(modifyUrl, values).Result;

		if (response == "")
		{
			return "Response was: Empty";
		}
		
		return response;
	}


	public async Task<string> PostAndGetResponse(string url, Dictionary<string, string> values)
	{
		var content = new FormUrlEncodedContent(values);
		
		var response = await client.PostAsync(url, content);

		var responseString = await response.Content.ReadAsStringAsync();
		return responseString;
	}
}

public class AccountData
{
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("login")]
	public string Login { get; set; }

	[JsonProperty("password")]
	public string Password { get; set; }

	[JsonProperty("stats")]
	public string Stats { get; set; }

	[JsonProperty("register_date")]
	public string RegisterDate { get; set; }
}

public class StatsData
{
	[JsonProperty("wins")]
	public string Wins = "0";
	
	[JsonProperty("mmr")]
	public string Mmr = "0";
}