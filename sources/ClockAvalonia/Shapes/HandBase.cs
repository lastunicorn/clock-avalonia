using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public abstract class HandBase : Shape, IHand
{
    #region Length StyledProperty

    public static readonly StyledProperty<double> LengthProperty = AvaloniaProperty.Register<HandBase, double>(
        nameof(Length),
        defaultValue: 95.0);

    public double Length
    {
        get => GetValue(LengthProperty);
        set => SetValue(LengthProperty, value);
    }

    #endregion

    #region TimeComponent StyledProperty

    public static readonly StyledProperty<TimeComponent> TimeComponentProperty = AvaloniaProperty.Register<HandBase, TimeComponent>(
        nameof(TimeComponent),
        defaultValue: TimeComponent.Second);

    public TimeComponent TimeComponent
    {
        get => GetValue(TimeComponentProperty);
        set => SetValue(TimeComponentProperty, value);
    }

    #endregion

    #region IntegralValue StyledProperty

    public static readonly StyledProperty<bool> IntegralValueProperty = AvaloniaProperty.Register<HandBase, bool>(
        nameof(IntegralValue),
        defaultValue: false);

    public bool IntegralValue
    {
        get => GetValue(IntegralValueProperty);
        set => SetValue(IntegralValueProperty, value);
    }

    #endregion

    static HandBase()
    {
        LengthProperty.Changed.AddClassHandler<HandBase>((hand, e) => hand.InvalidateCache());
    }

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (Length <= 0)
            return false;

        if (TimeComponent == TimeComponent.None)
            return false;

        return base.OnRendering(context);
    }

    protected double CalculateHandAngle(TimeSpan time)
    {
        if (IntegralValue)
        {
            return TimeComponent switch
            {
                TimeComponent.Hour => (time.Hours % 12) * 30.0,
                TimeComponent.Minute => time.Minutes * 6.0,
                TimeComponent.Second => time.Seconds * 6.0,
                _ => 0
            };
        }

        return TimeComponent switch
        {
            TimeComponent.Hour => (time.TotalHours % 12 / 12) * 360.0,
            TimeComponent.Minute => (time.TotalMinutes % 60 / 60) * 360.0,
            TimeComponent.Second => (time.TotalSeconds % 60 / 60) * 360.0,
            _ => 0
        };
    }
}
