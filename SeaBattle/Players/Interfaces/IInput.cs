using SeaBattle.Types;

namespace SeaBattle.Players.Interfaces;

public interface IInput
{
	public IntegerVector2 GetCoordinates(Player attacker, Player defender);
}