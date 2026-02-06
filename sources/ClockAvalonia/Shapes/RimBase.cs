using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public abstract class RimBase : Shape
{
    #region DistanceFromEdge StyledProperty

    public static readonly StyledProperty<double> DistanceFromEdgeProperty = AvaloniaProperty.Register<RimBase, double>(
        nameof(DistanceFromEdge),
        defaultValue: 0.0);

    public double DistanceFromEdge
    {
        get => GetValue(DistanceFromEdgeProperty);
        set => SetValue(DistanceFromEdgeProperty, value);
    }

    #endregion

    #region Angle StyledProperty

    public static readonly StyledProperty<double> AngleProperty = AvaloniaProperty.Register<RimBase, double>(
        nameof(Angle),
        defaultValue: 30.0);

    public double Angle
    {
        get => GetValue(AngleProperty);
        set => SetValue(AngleProperty, value);
    }

    #endregion

    #region OffsetAngle StyledProperty

    public static readonly StyledProperty<double> OffsetAngleProperty = AvaloniaProperty.Register<RimBase, double>(
        nameof(OffsetAngle),
        defaultValue: 0.0);

    public double OffsetAngle
    {
        get => GetValue(OffsetAngleProperty);
        set => SetValue(OffsetAngleProperty, value);
    }

    #endregion

    #region MaxCoverageCount StyledProperty

    public static readonly StyledProperty<uint> MaxCoverageCountProperty = AvaloniaProperty.Register<RimBase, uint>(
        nameof(MaxCoverageCount),
        defaultValue: 0u);

    public uint MaxCoverageCount
    {
        get => GetValue(MaxCoverageCountProperty);
        set => SetValue(MaxCoverageCountProperty, value);
    }

    #endregion

    #region MaxCoverageAngle StyledProperty

    public static readonly StyledProperty<uint> MaxCoverageAngleProperty = AvaloniaProperty.Register<RimBase, uint>(
        nameof(MaxCoverageAngle),
        defaultValue: 360u);

    public uint MaxCoverageAngle
    {
        get => GetValue(MaxCoverageAngleProperty);
        set => SetValue(MaxCoverageAngleProperty, value);
    }

    #endregion

    #region Orientation StyledProperty

    public static readonly StyledProperty<RimItemOrientation> OrientationProperty = AvaloniaProperty.Register<RimBase, RimItemOrientation>(
        nameof(Orientation),
        defaultValue: RimItemOrientation.FaceCenter);

    public RimItemOrientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    #endregion

    #region SkipIndex StyledProperty

    public static readonly StyledProperty<int> SkipIndexProperty = AvaloniaProperty.Register<RimBase, int>(
        nameof(SkipIndex),
        defaultValue: 0);

    public int SkipIndex
    {
        get => GetValue(SkipIndexProperty);
        set => SetValue(SkipIndexProperty, value);
    }

    #endregion

    protected override void DoRender(ClockDrawingContext context)
    {
        double radius = context.ClockRadius;
        double actualDistanceFromEdge = radius * DistanceFromEdge / 100.0;
        double itemRadius = radius - actualDistanceFromEdge;

        int index = 0;
        double angleDegrees = OffsetAngle + (index * Angle);

        while (angleDegrees >= 0)
        {
            if (MaxCoverageCount > 0 && index >= MaxCoverageCount)
                break;

            if (MaxCoverageAngle > 0 && angleDegrees - OffsetAngle >= MaxCoverageAngle)
                break;

            bool shouldSkip = SkipIndex > 0 && (index + 1) % SkipIndex == 0;

            if (!shouldSkip)
            {
                DrawingPlan.Create(context.DrawingContext)
                    .WithTransform(() => new RotateTransform(angleDegrees))
                    .WithTransform(() => new TranslateTransform(0, -itemRadius))
                    .WithTransform(() => CreateOrientationTransform(index))
                    .Draw(cd => RenderItem(context, index));
            }

            index++;
            angleDegrees = OffsetAngle + (index * Angle);
        }
    }

    private RotateTransform CreateOrientationTransform(int index)
    {
        switch (Orientation)
        {
            case RimItemOrientation.Normal:
                {
                    double totalAngle = -(OffsetAngle + Angle * index);
                    RotateTransform rotateTransform = new(totalAngle);
                    return rotateTransform;
                }

            case RimItemOrientation.FaceOut:
                {
                    RotateTransform rotateTransform = new(180);
                    return rotateTransform;
                }

            case RimItemOrientation.FaceCenter:
            default:
                return null;
        }
    }

    protected abstract void RenderItem(ClockDrawingContext context, int index);
}
