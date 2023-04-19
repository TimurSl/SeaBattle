using System.Numerics;

namespace SeaBattle;

public interface IPlayer
{
	public Map GetDefenseMap();
	public Map GetAttackMap();
	public IntegerVector2 GetTarget(Cell[,] attackMap);
	public string ToString();

	public virtual void Attack(IPlayer target, IntegerVector2 coordinates)
	{
		// check coordinates
		if (coordinates.X < 0 || coordinates.X >= Configuration.size || coordinates.Y < 0 || coordinates.Y >= Configuration.size)
		{
			Console.WriteLine("Invalid coordinates");
			return;
		}
		Cell targetCell = target.GetDefenseMap().Grid[coordinates.X, coordinates.Y];
		
		// attack target at attacker attack map
		GetAttackMap ().Grid[coordinates.X, coordinates.Y].ProcessAttackHit (target.GetDefenseMap (), coordinates);
		
		// attack target at target defense map
		targetCell.ProcessDefenseHit ();

		if (targetCell.CellType == Configuration.CellType.Ship)
		{
			Ship ship = (Ship) targetCell;
			if (!ship.IsAlive ())
			{
				GetAttackMap ().OutlineShip(ship);
			}
		}
		
		target.GetDefenseMap ().lastHit = coordinates;
	}
	public string GetName();


}