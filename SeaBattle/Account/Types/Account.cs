namespace SeaBattle.Account.Providers;

public struct Account
{
	public string Login { get; set; }
	public string Password { get; set; }

	public Stats Stats { get; set; }

	public Account(string login, string password, Stats stats)
	{
		Login = login;
		Password = password;
		Stats = stats;
	}
	
	public static bool operator ==(Account a, Account b)
	{
		return a.Login == b.Login && a.Password == b.Password;
	}

	public static bool operator !=(Account a, Account b)
	{
		return !(a == b);
	}
}