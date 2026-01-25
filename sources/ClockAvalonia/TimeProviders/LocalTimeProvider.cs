namespace DustInTheWind.ClockAvalonia.TimeProviders;

public class LocalTimeProvider : TimeProviderBase
{
    protected override TimeSpan GetTime()
    {
        return DateTime.Now.TimeOfDay;
    }
}
