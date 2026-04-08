public interface IRandomProvider
{
    bool Roll(float chance);
    int Range(int min, int max);
}