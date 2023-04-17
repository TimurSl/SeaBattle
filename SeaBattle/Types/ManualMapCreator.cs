namespace SeaBattle.Types;

public static class ManualMapCreator
{
	public static Cell[,] GetLevel()
	{
		Cell[,] map = LevelGenerator.MakeEmptyMap();
		
		return map;
	}
}