namespace DustInTheWind.ClockAvalonia.TimeProviders;

/// <summary>
/// Provides the system's local time.
/// </summary>
public class LocalTimeProvider : TimeProviderBase
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
