using SeaBattle.Players.Interfaces;
using SeaBattle.Settings;

namespace SeaBattle.Players.Types;

public struct PlayerParams
{
	public string Name;
	public PlayerType Type;
	public IInput Input;
	public Account.Providers.Account Account;
}