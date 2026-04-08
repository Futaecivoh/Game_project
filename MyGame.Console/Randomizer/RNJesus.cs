public static class RNJesus
{
    private static Random _random = new Random();
    
    public static bool Roll(float chance)
    {
        return _random.NextDouble() <= chance;
    }

    public static int Range(int min, int max)
    { 
        return _random.Next(min, max + 1); 
    }
}