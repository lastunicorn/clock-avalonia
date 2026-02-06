using System.ComponentModel;

namespace DustInTheWind.ClockAvalonia.Movements;

/// <summary>
/// Implements base functionality for a time provider class.
/// </summary>
public abstract class MovementBase : IMovement
{
    private readonly Timer timer;

    #region TickInterval Property

    private int tickInterval = 100;

    /// <summary>
    /// Gets or sets the interval in milliseconds at which the time provider generates time values.
    /// </summary>
    /// <remarks>
    /// Smaller numbers will generate more values per second, making the second hand move more
    /// smoothly, but will consume more processor time.
    /// An interval of 10 ms will make the clock move really smooth, but check also the performance
    /// on your computer.
    /// </remarks>
    [Category("Behavior")]
    [DefaultValue(100)]
    [Description("The interval in milliseconds at which the time provider generates time values.")]
    public int TickInterval
    {
        get => tickInterval;
        set
        {
            tickInterval = value;

            if (IsRunning)
            {
                if (tickInterval > 0)
                    timer.Change(0, tickInterval);
                else
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }
    }

    #endregion

    /// <summary>
    /// Gets a value indicating whether the time provider is currently running.
    /// </summary>
    [Browsable(false)]
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Gets the most recently provided value.
    /// </summary>
    [Browsable(false)]
    public TimeSpan LastTick { get; private set; }

    /// <summary>
    /// Occurs when the object is modified.
    /// </summary>
    /// <remarks>
    /// Subscribers can use this event to respond to changes in the object's state or content. The
    /// event is typically raised after a modification operation completes.
    /// </remarks>
    public event EventHandler Modified;

    /// <summary>
    /// Event raised when the time provider produces a new time value.
    /// </summary>
    public event EventHandler<TickEventArgs> Tick;

    /// <summary>
    /// Initializes a new instance of the <see cref="MovementBase"/> class.
    /// </summary>
    protected MovementBase()
    {
        timer = new Timer(HandleTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
    }

    private void HandleTimerCallback(object state)
    {
        LastTick = GenerateNewTime();
        OnTick(new TickEventArgs(LastTick));
    }

    /// <summary>
    /// Generates a new time value. This method is called internally by the timer.
    /// </summary>
    /// <returns>A <see cref="TimeSpan"/> object containing the time value.</returns>
    protected abstract TimeSpan GenerateNewTime();

    /// <summary>
    /// Starts the time provider. The time provider will begin generating time values.
    /// </summary>
    public void Start()
    {
        if (tickInterval > 0)
        {
            timer.Change(0, tickInterval);
        }
        else
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            ForceTick();
        }

        IsRunning = true;
    }

    /// <summary>
    /// Stops the time provider. The time provider will stop generating time values.
    /// </summary>
    public void Stop()
    {
        timer.Change(Timeout.Infinite, Timeout.Infinite);
        IsRunning = false;
    }

    /// <summary>
    /// Forces the timer to perform a tick operation immediately, updating the last tick time and raising the tick
    /// event.
    /// </summary>
    /// <remarks>
    /// This method bypasses any scheduled timing and triggers the tick logic as if the timer
    /// interval had elapsed. It can be used to manually advance the timer state or to simulate a tick for testing or
    /// synchronization purposes.
    /// </remarks>
    protected void ForceTick()
    {
        LastTick = GenerateNewTime();
        OnTick(new TickEventArgs(LastTick));
    }

    /// <summary>
    /// Raises the Modified event to notify subscribers that the object has been changed.
    /// </summary>
    /// <remarks>Derived classes can override this method to provide additional behavior when the object is
    /// modified. This method is typically called after a change to the object's state that should trigger
    /// notification.</remarks>
    protected virtual void OnModified()
    {
        Modified?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Raises the <see cref="Tick"/> event.
    /// </summary>
    /// <param name="e">A <see cref="TickEventArgs"/> object that contains the event data.</param>
    protected virtual void OnTick(TickEventArgs e)
    {
        Tick?.Invoke(this, e);
    }

    private bool disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                timer.Dispose();
                IsRunning = false;
            }

            disposed = true;
        }
    }

    ~MovementBase()
    {
        Dispose(false);
    }
}
