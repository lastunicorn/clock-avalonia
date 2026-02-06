using System.ComponentModel;

namespace DustInTheWind.ClockAvalonia.Movements;

/// <summary>
/// Provides time values from a time coordinate that is n times faster than the real one.
/// The n value may be negative, in which case the closk runs backwards.
/// </summary>
[Movement("Speedy", "Provides time that moves faster or slower than real time using a configurable multiplier.")]
public class SpeedyMovement : MovementBase
{
    private DateTime initialRealTime = DateTime.UtcNow;

    #region InitialTime Property

    private TimeSpan initialTime = DateTime.Now.TimeOfDay;

    /// <summary>
    /// Gets or sets the initial time value used as a reference for calculating subsequent time values.
    /// When this value is set, it resets the time calculation and restarts the provider.
    /// </summary>
    [Category("Behavior")]
    [Description("The initial time value used as a reference for calculating subsequent time values. When this value is set, it resets the time calculation and restarts the provider.")]
    public TimeSpan InitialTime
    {
        get => initialTime;
        set
        {
            if (initialTime == value)
                return;

            initialTime = value;

            initialRealTime = DateTime.UtcNow;
            OnModified();

            ForceTick();
        }
    }

    #endregion

    #region TimeMultiplier Property

    /// <summary>
    /// The default value of the time multiplier.
    /// </summary>
    public const float DefaultTimeMultiplier = 10;

    private float timeMultiplier = DefaultTimeMultiplier;

    /// <summary>
    /// Gets or sets the time multiplier that specifies how much faster the is the time compared
    /// to the real one.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(DefaultTimeMultiplier)]
    [Description("The time multiplier that specifies how much faster the is the time compared to the real one.")]
    public float TimeMultiplier
    {
        get => timeMultiplier;
        set
        {
            if (timeMultiplier == value)
                return;

            initialTime = GenerateNewTime();
            initialRealTime = DateTime.UtcNow;

            timeMultiplier = value;
            OnModified();

            ForceTick();
        }
    }

    #endregion

    /// <summary>
    /// Returns a new time value calculated based on the time multiplier.
    /// </summary>
    /// <returns>A <see cref="TimeSpan"/> object containing the time value.</returns>
    protected override TimeSpan GenerateNewTime()
    {
        DateTime currentRealTime = DateTime.UtcNow;
        long realDeltaTicks = currentRealTime.Ticks - initialRealTime.Ticks;
        double fakeDeltaTicks = realDeltaTicks * TimeMultiplier;
        TimeSpan fakeDelta = TimeSpan.FromTicks((long)fakeDeltaTicks);
        TimeSpan fakeTime = initialTime + fakeDelta;

        return fakeTime.Days > 0
            ? fakeTime.Subtract(TimeSpan.FromDays(fakeTime.Days))
            : fakeTime;
    }
}
