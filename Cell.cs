using static SeaBattle.Configuration;

namespace SeaBattle;

public class Cell
{
	public IntegerVector2 Position;
	public CellType CellType;

	public Cell(IntegerVector2 position, CellType cellType)
	{
		Position = position;
		CellType = cellType;
	}

	public Cell(IntegerVector2 position)
	{
		Position = position;
		CellType = CellType.Nothing;
	}

	public virtual void ProcessHit()
	{
		CellType = CellType.Miss;
	}
	
	public virtual bool IsShip()
	{
		return CellType == CellType.Ship;
	}
	
	public virtual char GetCellChar()
	{
		return PixelMap[(int) CellType].Char;
	}
	
	public virtual ConsoleColor GetCellColor()
	{
		return PixelMap[(int) CellType].Color;
	}
	
	public bool IsHitOrMiss()
	{
		return CellType == CellType.Hit || CellType == CellType.Miss;
	}
	
	public virtual void ProcessHit(Map map, IntegerVector2 coords)
	{
		Cell cell = map.Grid[coords.X, coords.Y];
		if (cell.CellType == CellType.Ship)
		{
			Console.WriteLine("Hit!");
			CellType = CellType.Hit;
		}
		else
		{
			Console.WriteLine("Miss!");
			CellType = CellType.Miss;
		}
	}
}

public class Ship : Cell
{
	public IntegerVector2 Position;
	public Cell[] ShipCells;
	public ShipType ShipType;
	
	public Ship(IntegerVector2 position, ShipType ship) : base(position)
	{
		Position = position;
		CellType = CellType.Ship;
		ShipType = ship;
	}
	
	public override void ProcessHit()
	{
		CellType = CellType.Hit;
	}
	
	public bool IsAlive()
	{
		return ShipCells.Any(cell => cell.CellType == CellType.Ship);
	}

	public override char GetCellChar()
	{
		if (CellType == CellType.Hit)
		{
			return PixelMap[(int) CellType.Hit].Char;
		}
		return ShipPixelMap[(int) ShipType].Char;
	}

	public override ConsoleColor GetCellColor()
	{
		if (CellType == CellType.Hit)
		{
			return PixelMap[(int) CellType.Hit].Color;
		}
		return ShipPixelMap[(int) ShipType].Color;
	}

	public override bool IsShip()
	{
		return true;
	}
}