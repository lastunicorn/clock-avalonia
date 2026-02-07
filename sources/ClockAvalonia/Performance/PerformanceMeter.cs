using System.Diagnostics;

namespace DustInTheWind.ClockAvalonia.Performance;

/// <summary>
/// Provides functionality to measure and track the execution time of code segments over multiple runs.
/// </summary>
/// <remarks>
/// PerformanceMeter can be used to measure the duration of repeated operations, aggregating statistics
/// such as total time, last measured time, and average time per measurement. The class is not thread-safe; if used from
/// multiple threads, callers should implement their own synchronization.
/// </remarks>
public class PerformanceMeter
{
    private readonly Stopwatch stopwatch = new();

    /// <summary>
    /// Gets the total number of measurements recorded.
    /// </summary>
    public long MeasurementCount { get; private set; }

    /// <summary>
    /// Gets the total elapsed time for the operation or process.
    /// </summary>
    public TimeSpan TotalTime { get; private set; }

    /// <summary>
    /// Gets the duration of the most recent operation.
    /// </summary>
    public TimeSpan LastTime { get; private set; }

    /// <summary>
    /// Gets the average duration of all recorded measurements.
    /// </summary>
    public TimeSpan AverageTime
    {
        get
        {
            long measurementCount = MeasurementCount;

            return measurementCount == 0
                ? TimeSpan.Zero
                : TotalTime / measurementCount;
        }
    }

    /// <summary>
    /// Event raised when the state of the object changes.
    /// </summary>
    public event EventHandler Changed;

    public void Measure(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        StartMeasurement();

        try
        {
            action();
        }
        finally
        {
            EndMeasurement();
        }
    }

    /// <summary>
    /// Starts or restarts the measurement step, resetting the internal timer to zero.
    /// </summary>
    /// <remarks>
    /// Use this method to begin measuring elapsed time or to restart the measurement from zero. If
    /// the timing operation is already running, calling this method resets the elapsed time and continues
    /// timing.
    /// </remarks>
    public void StartMeasurement()
    {
        stopwatch.Restart();
    }

    /// <summary>
    /// Ends the current measurement step and updates the measurement statistics.
    /// </summary>
    /// <remarks>
    /// Call this method after a measurement period to record the elapsed time and update related
    /// counters. This method should be paired with the corresponding method that starts the measurement interval. After
    /// calling this method, properties such as the last measured time, total accumulated time, and measurement count
    /// are updated to reflect the latest measurement.
    /// </remarks>
    public void EndMeasurement()
    {
        stopwatch.Stop();

        MeasurementCount++;
        LastTime = stopwatch.Elapsed;
        TotalTime += LastTime;

        OnChanged();
    }

    /// <summary>
    /// Resets all measurement data to their initial values.
    /// </summary>
    /// <remarks>
    /// Call this method to clear any accumulated measurement results and start a new measurement
    /// cycle. After calling this method, all related properties such as measurement count and total time are set to
    /// zero, and any change notifications are triggered as appropriate.
    /// </remarks>
    public void Reset()
    {
        MeasurementCount = 0;
        TotalTime = TimeSpan.Zero;
        LastTime = TimeSpan.Zero;

        OnChanged();
    }

    protected virtual void OnChanged()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
