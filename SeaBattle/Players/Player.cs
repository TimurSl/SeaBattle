using System.Text.RegularExpressions;
using SeaBattle.Cells;
using SeaBattle.MapCreators.Types;
using SeaBattle.Players.Inputs;
using SeaBattle.Players.Interfaces;
using SeaBattle.Players.Types;
using SeaBattle.Settings;
using SeaBattle.Types;

namespace SeaBattle.Players;

public class Player
{
	public string Name { get; set; }
	public Map DefenseMap;
	public Map AttackMap;
	private IntegerVector2 cursorPosition = new IntegerVector2(0, 0);
	private PlayerType type;

	public IInput Input;
	public bool IsStreak = false;

	public Player(PlayerParams @params)
	{
		this.type = @params.Type;
		Name = @params.Name;
		DefenseMap = new Map(LevelCreationType.Random, useLastHit:true);
		AttackMap = new Map(LevelCreationType.Empty, true);
		Input = @params.Input;
	}
	
	public virtual void Attack(Player target, IntegerVector2 coordinates)
	{
		// check coordinates
		if (coordinates.X < 0 || coordinates.X >= Configuration.size || coordinates.Y < 0 || coordinates.Y >= Configuration.size)
		{
			Console.WriteLine("Invalid coordinates");
			return;
		}
		Cell targetCell = target.DefenseMap.Grid[coordinates.X, coordinates.Y];
		
		// attack target at attacker attack map
		AttackMap.Grid[coordinates.X, coordinates.Y].ProcessAttackHit (target.DefenseMap, coordinates);
		
		// attack target at target defense map
		targetCell.ProcessDefenseHit ();

		Console.WriteLine($"Attacking {target.Name} at {coordinates.X}, {coordinates.Y}, new status: {targetCell.CellType}");

		if (targetCell is Ship ship)
		{
			if (!ship.IsAlive ())
			{
				AttackMap.OutlineShip(ship);
			}
			IsStreak = true;
		}
		else
		{
			IsStreak = false;
		}
		
		target.DefenseMap.lastHit = coordinates;
	}

	public IntegerVector2 GetTarget(Player attacker, Player defender)
	{
		return Input.GetCoordinates(attacker, defender);
	}

	public override string ToString()
	{
		return "Player";
	}


	public string GetName()
	{
		return Name;
	}
	
	public bool IsBot()
	{
		return type == PlayerType.Bot;
	}
}