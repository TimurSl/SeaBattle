// https://gist.github.com/jaykang920/8234457

using System.Security.Cryptography;

namespace SeaBattle.Types;

/// <summary>
/// A fast thread-safe wrapper for the default pseudo-random number generator.
/// </summary>
public static class SafeRandom
{
	private static Random _random;
	
	
	public static int Next()
	{
		return _random.Next();
	}
	
	public static int Next(int maxValue)
	{
		return _random.Next(maxValue);
	}
	
	public static int Next(int minValue, int maxValue)
	{
		return _random.Next(minValue, maxValue);
	}
	
	
	
}