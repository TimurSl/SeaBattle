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
	
	public bool IsAlreadyDestroyed()
	{
		return CellType == CellType.Hit || CellType == CellType.Miss;
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