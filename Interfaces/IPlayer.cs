using System.Numerics;

namespace SeaBattle;

public interface IPlayer
{
	public Map GetDefenseMap();
	public Map GetAttackMap();
	public IntegerVector2 GetTarget(Cell[,] attackMap);
	public string ToString();
	protected int MapSeed { get; set; }
	
	public virtual void Attack(IPlayer target, IntegerVector2 coordinates)
	{
		// check coordinates
		if (coordinates.X < 0 || coordinates.X >= Configuration.size || coordinates.Y < 0 || coordinates.Y >= Configuration.size)
		{
			Console.WriteLine("Invalid coordinates");
			return;
		}
		Cell targetCell = target.GetDefenseMap().Grid[coordinates.X, coordinates.Y];
		
		GetAttackMap ().Grid[coordinates.X, coordinates.Y].ProcessHit (target.GetDefenseMap (), coordinates);
		targetCell.ProcessHit ();

		if (targetCell.IsShip ())
		{
			Ship ship = (Ship) targetCell;
			if (!ship.IsAlive ())
			{
				GetAttackMap ().OutlineShip(ship);
			}
		}
	}
	public string GetName();


}