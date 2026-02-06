namespace DustInTheWind.ClockAvalonia.Movements;

/// <summary>
/// Provides random time values.
/// </summary>
[Movement("Random", "Provides random time values, useful for testing and demonstrations.")]
public class RandomTimeMovement : MovementBase
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
