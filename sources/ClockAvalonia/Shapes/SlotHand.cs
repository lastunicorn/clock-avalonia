using Avalonia;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Utils;

namespace DustInTheWind.ClockAvalonia.Shapes;

/// <summary>
/// A clock's hand that is actually a big disk with a rectangle slot carved in it through
/// which the user can see whatever is under the disk. Usually, the hours would be visible under the slot.
/// </summary>
public class SlotHand : HandBase
{
    /// <summary>
    /// The default name for the hand.
    /// </summary>
    public const string DefaultName = "Slot Hand";

    #region Width Styled Property

    public static readonly StyledProperty<double> WidthProperty = AvaloniaProperty.Register<SlotHand, double>(
        nameof(Width),
        defaultValue: 20.0);

    public double Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    #endregion

    #region Radius Styled Property

    public static readonly StyledProperty<double> RadiusProperty = AvaloniaProperty.Register<SlotHand, double>(
        nameof(Radius),
        defaultValue: 100.0);

    public double Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    #endregion

    #region TailLength Styled Property

    public static readonly StyledProperty<double> TailLengthProperty = AvaloniaProperty.Register<SlotHand, double>(
        nameof(TailLength),
        defaultValue: 0.0);

    public double TailLength
    {
        get => GetValue(TailLengthProperty);
        set => SetValue(TailLengthProperty, value);
    }

    #endregion

    static SlotHand()
    {
        WidthProperty.Changed.AddClassHandler<SlotHand>(HandleWidthChanged);
        RadiusProperty.Changed.AddClassHandler<SlotHand>(HandleRadiusChanged);
        TailLengthProperty.Changed.AddClassHandler<SlotHand>(HandleTasilLengthChanged);
    }

    private static void HandleWidthChanged(SlotHand hand, AvaloniaPropertyChangedEventArgs e)
    {
        hand.InvalidateCache();
        hand.OnChanged(EventArgs.Empty);
    }

    private static void HandleRadiusChanged(SlotHand hand, AvaloniaPropertyChangedEventArgs args)
    {
        hand.InvalidateCache();
        hand.OnChanged(EventArgs.Empty);
    }

    private static void HandleTasilLengthChanged(SlotHand hand, AvaloniaPropertyChangedEventArgs args)
    {
        hand.InvalidateCache();
        hand.OnChanged(EventArgs.Empty);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SlotHand"/> class with
    /// default values.
    /// </summary>
    public SlotHand()
    {
        Name = DefaultName;
    }

    /// <summary>
    /// Performs all the necessary calculations based on the public parameters, before drawing the shape.
    /// </summary>
    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        double radius = context.ClockRadius;

        StreamGeometry geometry = new();
        StreamGeometryContext geometryContext = geometry.Open();

        // Circular Disk

        double calculatedDiskRadius = Radius.RelativeTo(radius);

        geometryContext.BeginFigure(new Point(-calculatedDiskRadius, 0), true);

        geometryContext.ArcTo(
            point: new Point(calculatedDiskRadius, 0),
            size: new Size(calculatedDiskRadius, calculatedDiskRadius),
            rotationAngle: 0,
            isLargeArc: true,
            sweepDirection: SweepDirection.Clockwise,
            isStroked: true);

        geometryContext.ArcTo(
            point: new Point(-calculatedDiskRadius, 0),
            size: new Size(calculatedDiskRadius, calculatedDiskRadius),
            rotationAngle: 0,
            isLargeArc: true,
            sweepDirection: SweepDirection.Clockwise,
            isStroked: true);

        // Rectangle Slot

        double calculatedLength = Length.RelativeTo(radius);
        double calculatedTailLength = TailLength.RelativeTo(radius);
        double calculatedWidth = Width.RelativeTo(radius);
        double calculatedHalfWidth = calculatedWidth / 2;

        Point rectanglePoint1 = new(-calculatedHalfWidth, calculatedTailLength);
        Point rectanglePoint2 = new(-calculatedHalfWidth, -calculatedLength);
        Point rectanglePoint3 = new(calculatedHalfWidth, -calculatedLength);
        Point rectanglePoint4 = new(calculatedHalfWidth, calculatedTailLength);

        geometryContext.BeginFigure(rectanglePoint1, true);
        geometryContext.LineTo(rectanglePoint2, true);
        geometryContext.LineTo(rectanglePoint3, true);
        geometryContext.LineTo(rectanglePoint4, true);

        // Finish

        this.geometry = geometry;
    }

    private StreamGeometry geometry;

    protected override void DoRenderHand(ClockDrawingContext context)
    {
        context.DrawingContext.DrawGeometry(FillBrush, null, geometry);
    }
}
