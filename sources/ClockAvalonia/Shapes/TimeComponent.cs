namespace DustInTheWind.ClockAvalonia.Shapes;

/// <summary>
/// Specifies a component of a time value.
/// </summary>
[Flags]
public enum TimeComponent
{
    /// <summary>
    /// Represents no component of the time value.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents the hour component of the time value.
    /// </summary>
    Hour = 1,

    /// <summary>
    /// Represents the minute component of the time value.
    /// </summary>
    Minute = 2,

    /// <summary>
    /// Represents the second component of the time value.
    /// </summary>
    Second = 4,

    /// <summary>
    /// Represents all components, hour, minute, second of the time value.
    /// </summary>
    All = 7
}
