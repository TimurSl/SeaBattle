namespace SeaBattle;

public class Ship : Cell
{
	public IntegerVector2 Position;
	public Cell[] ShipCells;
	public Configuration.ShipType ShipType;
	
	public Ship(IntegerVector2 position, Configuration.ShipType ship) : base(position)
	{
		Position = position;
		CellType = Configuration.CellType.Ship;
		ShipType = ship;
	}
	
	public override void ProcessHit()
	{
		CellType = Configuration.CellType.Hit;
	}
	
	public bool IsAlive()
	{
		return ShipCells.Any(cell => cell.CellType == Configuration.CellType.Ship);
	}

	public override char GetCellChar()
	{
		if (CellType == Configuration.CellType.Hit)
		{
			return Configuration.PixelMap[(int) Configuration.CellType.Hit].Char;
		}
		return Configuration.ShipPixelMap[(int) ShipType].Char;
	}

	public override ConsoleColor GetCellColor()
	{
		if (CellType == Configuration.CellType.Hit)
		{
			return Configuration.PixelMap[(int) Configuration.CellType.Hit].Color;
		}
		return Configuration.ShipPixelMap[(int) ShipType].Color;
	}

	public override bool IsShip()
	{
		return true;
	}
}