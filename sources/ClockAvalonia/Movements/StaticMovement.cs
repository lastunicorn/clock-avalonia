using System.ComponentModel;

namespace DustInTheWind.ClockAvalonia.Movements;

/// <summary>
/// Provides a movement that returns a fixed time value, with optional smooth transitions when the value changes.
/// </summary>
/// <remarks>
/// Use StaticMovement to supply a constant time value that can be updated at runtime. When the time
/// value is changed, the transition to the new value can occur instantly or be smoothly interpolated over a specified
/// interval, depending on the TransitionInterval property. This class is useful for scenarios where a time value needs
/// to be controlled directly, with optional animation between changes.
/// </remarks>
[Movement("Static", "Provides a fixed time value with optional smooth transition when changed.")]
public class StaticMovement : MovementBase
{
    private readonly Timer transitionTimer;
    private TimeSpan targetTime = DateTime.Now.TimeOfDay;
    private TimeSpan currentTime = DateTime.Now.TimeOfDay;
    private TimeSpan startTime;
    private DateTime transitionStartTime;
    private bool isTransitioning;

    #region Time Property

    /// <summary>
    /// Gets or sets the desired time value to be used.
    /// </summary>
    [Category("Behavior")]
    [Description("The desired time value to be used.")]
    public TimeSpan Time
    {
        get => targetTime;
        set
        {
            if (targetTime == value)
                return;

            targetTime = value;
            OnModified();

            StartTransition();
        }
    }

    #endregion

    #region TransitionInterval Property

    private TimeSpan transitionInterval = TimeSpan.Zero;

    /// <summary>
    /// Gets or sets the duration over which the time value transitions to a new value in real time.
    /// </summary>
    /// <remarks>
    /// Setting this property to a negative value will automatically reset it to zero. Use this
    /// property to control the smoothness or speed of time-based transitions.
    /// </remarks>
    [Category("Behavior")]
    [Description("The real-time duration over which the Time value transitions to the new value.")]
    public TimeSpan TransitionInterval
    {
        get => transitionInterval;
        set
        {
            if (value < TimeSpan.Zero)
                value = TimeSpan.Zero;

            transitionInterval = value;
            OnModified();
        }
    }

    #endregion

    #region TransitionTickInterval Property

    private int transitionTickInterval = 30;

    /// <summary>
    /// Gets or sets the real-time interval, in ticks, over which the Time value transitions to a new value.
    /// </summary>
    [Category("Behavior")]
    [Description("The real-time duration over which the Time value transitions to the new value.")]
    public int TransitionTickInterval
    {
        get => transitionTickInterval;
        set
        {
            if (value == transitionTickInterval)
                return;

            transitionTickInterval = value;
            OnModified();
        }
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the StaticMovement class with default settings.
    /// </summary>
    /// <remarks>
    /// This constructor sets the TickInterval property to 0 and prepares the internal timer for use.
    /// The timer does not start automatically upon construction.
    /// </remarks>
    public StaticMovement()
    {
        TickInterval = 0;
        transitionTimer = new Timer(HandleTransitionTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
    }

    private void StartTransition()
    {
        if (transitionInterval <= TimeSpan.Zero || transitionTickInterval <= 0)
        {
            currentTime = targetTime;
            ForceTick();
            return;
        }

        startTime = currentTime;
        transitionStartTime = DateTime.Now;
        isTransitioning = true;

        if (TickInterval == 0)
            _ = transitionTimer.Change(0, transitionTickInterval);

        ForceTick();
    }

    private void HandleTransitionTimerCallback(object state)
    {
        if (!isTransitioning)
            return;

        ForceTick();
    }

    /// <summary>
    /// Calculates and returns the current time value, progressing toward the target time if a transition is in
    /// progress.
    /// </summary>
    /// <remarks>
    /// This method is typically called to update or retrieve the current time during a transition.
    /// When the transition completes, the returned value equals the target time and the transition state is
    /// reset.
    /// </remarks>
    /// <returns>A TimeSpan representing the current time. If a transition is in progress, the value reflects the interpolated
    /// time between the start and target times; otherwise, it returns the current time without modification.</returns>
    protected override TimeSpan GenerateNewTime()
    {
        if (!isTransitioning)
            return currentTime;

        TimeSpan elapsed = DateTime.Now - transitionStartTime;

        if (elapsed >= transitionInterval)
        {
            currentTime = targetTime;
            isTransitioning = false;

            if (TickInterval == 0)
                _ = transitionTimer.Change(Timeout.Infinite, Timeout.Infinite);

            return currentTime;
        }

        double progress = elapsed.TotalMilliseconds / transitionInterval.TotalMilliseconds;
        double startTicks = startTime.Ticks;
        double targetTicks = targetTime.Ticks;
        double currentTicks = startTicks + (targetTicks - startTicks) * progress;

        currentTime = TimeSpan.FromTicks((long)currentTicks);
        return currentTime;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _ = transitionTimer.Change(Timeout.Infinite, Timeout.Infinite);
            transitionTimer.Dispose();
        }

        base.Dispose(disposing);
    }
}
