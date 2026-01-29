namespace DustInTheWind.ClockAvalonia.TimeProviders;

/// <summary>
/// Provides random time values.
/// </summary>
public class RandomTimeProvider : TimeProviderBase
{
    /// <summary>
    /// Returns a random time value.
    /// </summary>
    /// <returns>A <see cref="TimeSpan"/> object containing a random time value.</returns>
    protected override TimeSpan GenerateNewTime()
    {
        long ticks = Random.Shared.NextInt64(TimeSpan.TicksPerDay);
        return TimeSpan.FromTicks(ticks);
    }
}
