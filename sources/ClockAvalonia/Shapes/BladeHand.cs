using Avalonia;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Utils;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class BladeHand : HandBase
{
    #region Width StyledProperty

    public static readonly StyledProperty<double> WidthProperty = AvaloniaProperty.Register<BladeHand, double>(
        nameof(Width),
        defaultValue: 20.0);

    public double Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    #endregion

    #region HipDistance StyledProperty

    public static readonly StyledProperty<double> HipDistanceProperty = AvaloniaProperty.Register<BladeHand, double>(
        nameof(HipDistance),
        defaultValue: 20.0);

    public double HipDistance
    {
        get => GetValue(HipDistanceProperty);
        set => SetValue(HipDistanceProperty, value);
    }

    #endregion

    #region ShadowMargin StyledProperty

    public static readonly StyledProperty<double> ShadowMarginProperty = AvaloniaProperty.Register<BladeHand, double>(
        nameof(ShadowMargin),
        defaultValue: 2.0);

    public double ShadowMargin
    {
        get => GetValue(ShadowMarginProperty);
        set => SetValue(ShadowMarginProperty, value);
    }

    #endregion

    static BladeHand()
    {
        StrokeThicknessProperty.OverrideDefaultValue<BladeHand>(0.0);

        WidthProperty.Changed.AddClassHandler<BladeHand>(HandleWidthChanged);
        HipDistanceProperty.Changed.AddClassHandler<BladeHand>(HipDistanceChanged);
        ShadowMarginProperty.Changed.AddClassHandler<BladeHand>(HandleShadowMarginChanged);
    }

    private static void HandleWidthChanged(BladeHand hand, AvaloniaPropertyChangedEventArgs args)
    {
        hand.InvalidateCache();
        hand.OnChanged(EventArgs.Empty);
    }

    private static void HipDistanceChanged(BladeHand hand, AvaloniaPropertyChangedEventArgs args)
    {
        hand.InvalidateCache();
        hand.OnChanged(EventArgs.Empty);
    }

    private static void HandleShadowMarginChanged(BladeHand hand, AvaloniaPropertyChangedEventArgs args)
    {
        hand.InvalidateCache();
        hand.OnChanged(EventArgs.Empty);
    }

    private StreamGeometry geometryBackground;
    private StreamGeometry geometryShade;
    private Pen strokePen;

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        double clockRadius = context.ClockRadius;
        double calculatedLength = Length.RelativeTo(clockRadius);
        double calculatedWidth = Width.RelativeTo(clockRadius);
        double calculatedHalfWidth = calculatedWidth / 2;
        double hipDistance = HipDistance.RelativeTo(clockRadius);

        // Background

        Point pointA1 = new(0, 0);
        Point pointA2 = new(-calculatedHalfWidth, -hipDistance);
        Point pointA3 = new(0, -calculatedLength);
        Point pointA4 = new(calculatedHalfWidth, -hipDistance);

        StreamGeometry geometryBackground = new();

        StreamGeometryContext streamGeometryContext1 = geometryBackground.Open();

        streamGeometryContext1.BeginFigure(pointA1, true);

        streamGeometryContext1.LineTo(pointA2, true);
        streamGeometryContext1.LineTo(pointA3, true);
        streamGeometryContext1.LineTo(pointA4, true);

        this.geometryBackground = geometryBackground;

        // Background - Stroke

        strokePen = CreateStrokePen();

        // Shadow

        double calculatedShadowMargin = ShadowMargin.RelativeTo(clockRadius);

        if (calculatedShadowMargin < 0)
            calculatedShadowMargin = 0;
        else if (calculatedShadowMargin > calculatedHalfWidth)
            calculatedShadowMargin = calculatedHalfWidth;

        Point pointB1 = new(0, -calculatedShadowMargin * 1.5);
        Point pointB2 = new(-calculatedHalfWidth + calculatedShadowMargin, -hipDistance);
        Point pointB3 = new(0, -calculatedLength + calculatedShadowMargin * 4);

        StreamGeometry geometryShade = new();

        StreamGeometryContext streamGeometryContext2 = geometryShade.Open();

        streamGeometryContext2.BeginFigure(pointB1, true);

        streamGeometryContext2.LineTo(pointB2, true);
        streamGeometryContext2.LineTo(pointB3, true);

        this.geometryShade = geometryShade;
    }

    protected override void DoRenderHand(ClockDrawingContext context)
    {
        context.DrawingContext.DrawGeometry(FillBrush, strokePen, geometryBackground);
        context.DrawingContext.DrawGeometry(Brushes.Gray, null, geometryShade);
    }
}
