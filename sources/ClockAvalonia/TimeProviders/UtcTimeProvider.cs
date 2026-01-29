using System.ComponentModel;

namespace DustInTheWind.ClockAvalonia.TimeProviders;

/// <summary>
/// Provides the UTC time. Optionally, an offset may be provided.
/// </summary>
public class UtcTimeProvider : TimeProviderBase
{
    /// <summary>
    /// Gets or sets the offset time used to adjust the system's UTC time value.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(typeof(TimeSpan), "00:00:00")]
    [Description("The offset time used to adjust the system's UTC time value.")]
    public TimeSpan UtcOffset
    {
        get => field;
        set
        {
            if(field == value)
                return;

            field = value;
            ForceTick();
        }
    }

    /// <summary>
    /// Returns the system's UTC time added with the offset value.
    /// </summary>
    /// <returns>A <see cref="TimeSpan"/> object containing the time value.</returns>
    protected override TimeSpan GenerateNewTime()
    {
        return UtcOffset == TimeSpan.Zero
            ? DateTime.UtcNow.TimeOfDay
            : DateTime.UtcNow.TimeOfDay.Add(UtcOffset);
    }
}
