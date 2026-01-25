namespace DustInTheWind.ClockAvalonia.Shapes;

[Flags]
public enum TimeComponent
{
    None = 0,
    Hour = 1,
    Minute = 2,
    Second = 4,
    All = 7
}
