namespace DustInTheWind.ClockAvalonia.TimeProviders;

/// <summary>
/// Provides the time to be displayed by a clock. The time is provided as <see cref="TimeSpan"/> objects.
/// </summary>
public interface ITimeProvider : IDisposable
{
    /// <summary>
    /// Event raised when the time provider produces a new time value.
    /// </summary>
    event EventHandler<TickEventArgs> Tick;

    /// <summary>
    /// Gets or sets the interval in milliseconds at which the time provider generates time values.
    /// </summary>
    int TickInterval { get; set; }

    /// <summary>
    /// Gets a value indicating whether the time provider is currently running.
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// Gets the most recently provided value.
    /// </summary>
    TimeSpan LastValue { get; }

    /// <summary>
    /// Starts the time provider. The time provider will begin generating time values.
    /// </summary>
    void Start();

    /// <summary>
    /// Stops the time provider. The time provider will stop generating time values.
    /// </summary>
    void Stop();
}
