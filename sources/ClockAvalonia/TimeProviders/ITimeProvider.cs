namespace DustInTheWind.ClockAvalonia.TimeProviders;

public interface ITimeProvider : IDisposable
{
    event EventHandler<TimeChangedEventArgs> TimeChanged;

    int Interval { get; set; }

    bool IsRunning { get; }

    TimeSpan LastValue { get; }

    void Start();

    void Stop();
}
