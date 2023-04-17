// https://gist.github.com/jaykang920/8234457

using System;
using System.Security.Cryptography;

/// <summary>
/// A fast thread-safe wrapper for the default pseudo-random number generator.
/// </summary>
public class SafeRandom
{
    // Global seed generator
    private static RNGCryptoServiceProvider global;

    // Thread-local pseudo-random generator
    [ThreadStatic]
    private static Random local;

    // Gets or initializes a thread-local Random instance.
    private static Random Local
    {
        get
        {
            Random random = local;
            if (random == null)
            {
                byte[] buffer = new byte[4];
                global.GetBytes(buffer);
                int seed = BitConverter.ToInt32(buffer, 0);
                random = local = new Random(seed);
            }
            return random;
        }
    }

    static SafeRandom()
    {
        global = new RNGCryptoServiceProvider();
    }
    
    /// <summary>
    /// Returns a nonnegative random number.
    /// </summary>
    public static int Next()
    {
        return Local.Next();
    }

    /// <summary>
    /// Returns a nonnegative random number less than the specified maximum.
    /// </summary>
    public int Next(int maxValue)
    {
        return Local.Next(maxValue);
    }

    /// <summary>
    /// Returns a random number within a specified range.
    /// </summary>
    public static int Next(int minValue, int maxValue)
    {
        return Local.Next(minValue, maxValue);
    }
}