namespace DustInTheWind.ClockAvalonia.Movements;

/// <summary>
/// Contains the event data for the <see cref="IMovement.Tick"/> event.
/// </summary>
public class TickEventArgs : EventArgs
{
    /// <summary>
    /// Gets the time value.
    /// </summary>
    public TimeSpan Time { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TickEventArgs"/> class.
    /// </summary>
    /// <param name="time">The time value.</param>
    public TickEventArgs(TimeSpan time)
    {
        Time = time;
    }
}
