using System;

public class RandomHelper
{
    private readonly Random random;

    public RandomHelper(int seed)
    {
        random = new Random(seed);
    }

    public bool Chance(float percent)
    {
        return random.NextDouble() < percent / 100f;
    }

    public int Range(int min, int max)
    {
        return random.Next(min, max);
    }

    public float Value()
    {
        return (float)random.NextDouble();
    }
}