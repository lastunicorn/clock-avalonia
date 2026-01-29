using System.ComponentModel;

namespace DustInTheWind.ClockAvalonia.TimeProviders;

/// <summary>
/// Returns a fixed time provided by the <see cref="Time"/> property.
/// </summary>
/// <remarks>
/// The <see cref="TickInterval"/> is, by default set to 0. This means that the instance will
/// not produce other times unless the <see cref="Time"/> property is manually changed.
/// </remarks>
public class StaticTimeProvider : TimeProviderBase
{
    #region Time Property

    private TimeSpan time = DateTime.Now.TimeOfDay;

    [Category("Behavior")]
    [Description("The desired time value to be returned.")]
    public TimeSpan Time
    {
        get => time;
        set
        {
            time = value;
            ForceTick();
        }
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="StaticTimeProvider"/> class.
    /// </summary>
    public StaticTimeProvider()
    {
        TickInterval = 0;
    }

    protected override TimeSpan GenerateNewTime()
    {
        return time;
    }
}
