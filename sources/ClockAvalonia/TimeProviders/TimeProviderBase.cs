namespace DustInTheWind.ClockAvalonia.TimeProviders;

public abstract class TimeProviderBase : ITimeProvider
{
    private readonly Timer timer;
    private int interval = 100;

    public int Interval
    {
        get => interval;
        set
        {
            interval = value;

            if (IsRunning)
                timer.Change(interval, interval);
        }
    }

    public bool IsRunning { get; private set; }

    public TimeSpan LastValue { get; private set; }

    public event EventHandler<TimeChangedEventArgs>? TimeChanged;

    protected TimeProviderBase()
    {
        timer = new Timer(HandleTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
    }

    private void HandleTimerCallback(object? state)
    {
        LastValue = GetTime();
        OnTimeChanged(new TimeChangedEventArgs(LastValue));
    }

    protected abstract TimeSpan GetTime();

    public void Start()
    {
        LastValue = GetTime();
        OnTimeChanged(new TimeChangedEventArgs(LastValue));

        timer.Change(interval, interval);
        IsRunning = true;
    }

    public void Stop()
    {
        timer.Change(Timeout.Infinite, Timeout.Infinite);
        IsRunning = false;
    }

    protected virtual void OnTimeChanged(TimeChangedEventArgs e)
    {
        TimeChanged?.Invoke(this, e);
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

    ~TimeProviderBase()
    {
        Dispose(false);
    }
}
