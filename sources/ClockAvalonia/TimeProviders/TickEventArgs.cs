namespace DustInTheWind.ClockAvalonia.TimeProviders;

public class TickEventArgs : EventArgs
{
    public TimeSpan Time { get; }

    public TickEventArgs(TimeSpan time)
    {
        Time = time;
    }
}
