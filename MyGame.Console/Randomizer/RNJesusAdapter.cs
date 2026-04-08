public class RNJesusAdapter : IRandomProvider
{
    public bool Roll(float chance)
    {
        return RNJesus.Roll(chance);
    }

    public int Range(int min, int max)
    {
        return RNJesus.Range(min, max);
    }
}