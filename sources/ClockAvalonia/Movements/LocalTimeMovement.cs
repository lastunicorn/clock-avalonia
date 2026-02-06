namespace DustInTheWind.ClockAvalonia.Movements;

/// <summary>
/// Provides the system's local time.
/// </summary>
[Movement("Local Time", "Provides the current local time from the system clock.")]
public class LocalTimeMovement : MovementBase
{
    /// <summary>
    /// Returns the system's local time from the moment of the request.
    /// </summary>
    /// <returns>A <see cref="TimeSpan"/> object containing the time value.</returns>
    protected override TimeSpan GenerateNewTime()
    {
        return DateTime.Now.TimeOfDay;
    }
}
