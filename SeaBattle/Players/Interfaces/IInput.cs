using SeaBattle.Players;
using SeaBattle.Players.Inputs;

namespace SeaBattle;

public interface IInput
{
	public IntegerVector2 GetCoordinates(Player attacker, Player defender);
}

public struct InputParams
{
	public BotDifficulties Difficulty { get; set; }
}