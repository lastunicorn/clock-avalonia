namespace DustInTheWind.ClockAvalonia.Shapes;

/// <summary>
/// Represents the angle of a clock hand for a specified time, time component, and rotation direction.
/// </summary>
/// <remarks>
/// A HandAngle instance encapsulates the calculation of the angle for a specific clock hand (hour,
/// minute, or second) at a given time. The angle can be computed in either clockwise or counterclockwise direction and
/// can be returned as an integral or fractional value, depending on the configuration. This type is immutable and can
/// be implicitly converted to a double representing the calculated angle in degrees.
/// </remarks>
public record class HandAngle
{
    private bool isValueCalculated;

    public required TimeSpan Time { get; init; }

    public required TimeComponent TimeComponent { get; init; }

    public required RotationDirection ClockDirection { get; init; }

    public required bool IntegralValue { get; init; }

    public double Value
    {
        get
        {
            if (!isValueCalculated)
            {
                field = CalculateAngle();
                isValueCalculated = true;
            }

            return field;
        }
    }

    private double CalculateAngle()
    {
        double angle = CalculateClockwiseHandAngle(Time);

        if (ClockDirection == RotationDirection.Counterclockwise)
            angle = -angle;

        return angle;
    }

    private double CalculateClockwiseHandAngle(TimeSpan time)
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

    public static implicit operator double(HandAngle handAngle)
    {
        return handAngle.Value;
    }
}
