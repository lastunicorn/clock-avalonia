namespace DustInTheWind.ClockAvalonia.Shapes;

public record class RimItemAngle
{
    private bool isValueCalculated;

    public required int Index { get; init; }

    public required RotationDirection ClockDirection { get; init; }

    public required double AngleBetweenItems { get; init; }

    public required double OffsetAngle { get; init; }

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

    public bool IsTopHalf
    {
        get
        {
            double normalizedAngle = Value % 360;

            if (ClockDirection == RotationDirection.Counterclockwise)
            {
                return normalizedAngle < -90 && normalizedAngle > -270;
            }
            else
            {
                return normalizedAngle > 90 && normalizedAngle < 270;
            }
        }
    }


    private double CalculateAngle()
    {
        double angle = OffsetAngle + AngleBetweenItems * Index;

        if (ClockDirection == RotationDirection.Counterclockwise)
            angle = -angle;

        return angle;
    }

    public bool IsBiggerOrEqualThan(double angle)
    {
        switch (ClockDirection)
        {
            case RotationDirection.Clockwise:
                return Value - OffsetAngle >= angle;

            case RotationDirection.Counterclockwise:
                return -Value - OffsetAngle >= angle;

            default:
                return false;
        }
    }

    public bool IsSmallerOrEqualThan(double angle)
    {
        switch (ClockDirection)
        {
            case RotationDirection.Clockwise:
                return Value - OffsetAngle <= angle;

            case RotationDirection.Counterclockwise:
                return -Value - OffsetAngle <= angle;

            default:
                return false;
        }
    }

    public static bool operator >=(RimItemAngle rimItemAngle, double angle)
    {
        return rimItemAngle.IsBiggerOrEqualThan(angle);
    }

    public static bool operator <=(RimItemAngle rimItemAngle, double angle)
    {
        return rimItemAngle.IsSmallerOrEqualThan(angle);
    }

    public static implicit operator double(RimItemAngle rimItemAngle)
    {
        return rimItemAngle.Value;
    }
}