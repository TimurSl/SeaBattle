using System.Numerics;

namespace SeaBattle;

public interface IPlayer
{
	public IntegerVector2 GetTarget(int[,] attackMap);
}