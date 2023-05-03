using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SeaBattle.Account.Providers;

public class XMLProvider : IAccountProvider
{
	public Account GetAccount(string login, string password)
	{
		string folder = "Profiles";
		if (!Directory.Exists(folder))
		{
			Directory.CreateDirectory(folder);
		}
		// combine absolute path to profile file, from disk root to current folder
		
		var path = Path.Combine(folder, $"{login}.xml");
		
		path = Path.GetFullPath(path);
		
		// Console.WriteLine(path);

		if (!File.Exists(path) || new FileInfo(path).Length == 0)
		{
			Account account = CreateAccountWithStats(login, password, new StatsData() { Wins = "0", Mmr = "0" });
			return account;
		}
		else
		{
			XmlSerializer serializer = new XmlSerializer(typeof(AccountData));
			using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
			{
				AccountData accountData = (AccountData) serializer.Deserialize(fs);
				Stats stats = JsonConvert.DeserializeObject<Stats>(accountData?.Stats);
				return new Account(login, password, stats, this);
			}
		}
	}
	

	public Account ModifyStats(string login, string password, StatsData newStats)
	{
		var path = Path.Combine("Profiles", $"{login}.xml");

		if (!File.Exists(path))
		{
			return CreateAccountWithStats (login, password, newStats);
		}
		
		if (new FileInfo(path).Length == 0)
		{
			return CreateAccountWithStats (login, password, newStats);
		}

		XmlSerializer serializer = new XmlSerializer(typeof(AccountData));
		using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
		{
			AccountData accountData = (AccountData) serializer.Deserialize(fs);
			Stats stats = JsonConvert.DeserializeObject<Stats>(accountData?.Stats);
				
			stats.Wins = int.Parse(newStats.Wins);
			stats.MMR = int.Parse(newStats.Mmr);
			accountData.Stats = JsonConvert.SerializeObject(stats);
			
			fs.SetLength(0);
			serializer.Serialize(fs, accountData);
				
			return new Account(login, password, stats, this);
		}

	}

	private Account CreateAccountWithStats(string login, string password, StatsData newStats)
	{
		string path = Path.Combine("Profiles", $"{login}.xml");
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(AccountData));
		using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
		{
			AccountData accountData = new AccountData ();
			accountData.Login = login;
			// accountData.Password = password;
			// serialize stats to XML, not JSON

			Stats stats = new Stats(int.Parse(newStats.Wins), int.Parse(newStats.Mmr));
			accountData.Stats = JsonConvert.SerializeObject(stats);
			xmlSerializer.Serialize(fs, accountData);

			return new Account(login, password, stats, this);
		}
	}
}