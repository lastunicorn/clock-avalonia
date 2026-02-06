namespace DustInTheWind.ClockAvalonia.Utils;

public static class DoubleExtensions
{
    public static double RelativeTo(this double value, double referenceValue)
    {
        return referenceValue * value / 100.0;
    }

    public static double RelativeTo(this int value, int referenceValue)
    {
        return referenceValue * value / 100.0;
    }

    public static double RelativeTo(this double value, int referenceValue)
    {
        return referenceValue * value / 100.0;
    }

    public static double RelativeTo(this int value, double referenceValue)
    {
        return referenceValue * value / 100.0;
    }
}
