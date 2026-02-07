using Avalonia;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Utils;

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
        defaultValue: RimItemOrientation.FaceIn);

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
        double calculatedDistanceFromEdge = DistanceFromEdge.RelativeTo(radius);
        double rimRadius = radius - calculatedDistanceFromEdge;

        int index = 0;

        RimItemAngle itemAngle = new()
        {
            Index = index,
            AngleBetweenItems = Angle,
            OffsetAngle = OffsetAngle,
            ClockDirection = context.ClockDirection
        };

        while (true)
        {
            if (MaxCoverageCount > 0 && index >= MaxCoverageCount)
                break;

            if (MaxCoverageAngle > 0 && itemAngle >= MaxCoverageAngle)
                break;

            bool shouldSkip = SkipIndex > 0 && (index + 1) % SkipIndex == 0;

            if (!shouldSkip)
            {
                DrawingPlan.Create(context.DrawingContext)
                    .WithTransform(() => new RotateTransform((double)itemAngle, 0, 0))
                    .WithTransform(() => new TranslateTransform(0, -rimRadius))
                    .WithTransform(() => CreateItemOrientationTransform(itemAngle))
                    .Draw(cd => RenderItem(context, index));
            }

            index++;

            itemAngle = new()
            {
                Index = index,
                AngleBetweenItems = Angle,
                OffsetAngle = OffsetAngle,
                ClockDirection = context.ClockDirection
            };
        }
    }

    private RotateTransform CreateItemOrientationTransform(RimItemAngle itemAngle)
    {
        switch (Orientation)
        {
            case RimItemOrientation.Normal:
                {
                    double rotationAngle = -(double)itemAngle;
                    return new RotateTransform(rotationAngle, 0, 0);
                }

            default:
            case RimItemOrientation.FaceIn:
                return null;

            case RimItemOrientation.FaceOut:
                return new RotateTransform(180, 0, 0);

            case RimItemOrientation.HalfInHalfOut:
                {
                    return itemAngle.IsTopHalf
                        ? new RotateTransform(180, 0, 0)
                        : null;
                }

            case RimItemOrientation.Custom:
                return OnItemOrientation(itemAngle);
        }
    }

    /// <summary>
    /// Provides a custom orientation transform for the item at the specified index.
    /// </summary>
    /// <remarks>Override this method to supply a specific orientation for individual items. The default
    /// implementation returns <c>null</c>, indicating no rotation is applied.</remarks>
    /// <returns>A <see cref="RotateTransform"/> representing the orientation of the item at the specified index, or <c>null</c>
    /// if no orientation is applied.</returns>
    protected virtual RotateTransform OnItemOrientation(RimItemAngle itemAngle)
    {
        return null;
    }

    /// <summary>
    /// Draws the item at the specified index using the provided drawing context.
    /// </summary>
    /// <remarks>
    /// This method is called once for each item that must be drawn around the clock face.
    /// The position and orientation of the item is already set when this method is called.
    /// The item should be drawn centered at the point (0,0).
    /// </remarks>
    /// <param name="context">The drawing context to use for rendering the item.</param>
    /// <param name="index">The zero-based index of the item to render.</param>
    protected abstract void RenderItem(ClockDrawingContext context, int index);
}
