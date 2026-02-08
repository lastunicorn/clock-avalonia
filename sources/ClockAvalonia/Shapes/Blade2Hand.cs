using Avalonia;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Utils;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class Blade2Hand : HandBase
{
    #region Width StyledProperty

    public static readonly StyledProperty<double> WidthProperty = AvaloniaProperty.Register<Blade2Hand, double>(
        nameof(Width),
        defaultValue: 20.0);

    public double Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    #endregion

    #region HipDistance StyledProperty

    public static readonly StyledProperty<double> HipDistanceProperty = AvaloniaProperty.Register<Blade2Hand, double>(
        nameof(HipDistance),
        defaultValue: 45);

    public double HipDistance
    {
        get => GetValue(HipDistanceProperty);
        set => SetValue(HipDistanceProperty, value);
    }

    #endregion

    #region TipLength StyledProperty

    public static readonly StyledProperty<double> TipLengthProperty = AvaloniaProperty.Register<Blade2Hand, double>(
        nameof(TipLength),
        defaultValue: 15.0);

    public double TipLength
    {
        get => GetValue(TipLengthProperty);
        set => SetValue(TipLengthProperty, value);
    }

    #endregion

    static Blade2Hand()
    {
        StrokeThicknessProperty.OverrideDefaultValue<Blade2Hand>(0.0);

        WidthProperty.Changed.AddClassHandler<Blade2Hand>(HandleWidthChanged);
        HipDistanceProperty.Changed.AddClassHandler<Blade2Hand>(HipDistanceChanged);
        TipLengthProperty.Changed.AddClassHandler<Blade2Hand>(HandleTipLengthChanged);
    }

    private static void HandleWidthChanged(Blade2Hand hand, AvaloniaPropertyChangedEventArgs e)
    {
        hand.InvalidateCache();
        hand.OnChanged(EventArgs.Empty);
    }

    private static void HipDistanceChanged(Blade2Hand hand, AvaloniaPropertyChangedEventArgs e)
    {
        hand.InvalidateCache();
        hand.OnChanged(EventArgs.Empty);
    }

    private static void HandleTipLengthChanged(Blade2Hand hand, AvaloniaPropertyChangedEventArgs e)
    {
        hand.InvalidateCache();
        hand.OnChanged(EventArgs.Empty);
    }

    private StreamGeometry geometry;
    private Pen strokePen;

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        double clockRadius = context.ClockRadius;
        double length = Length.RelativeTo(clockRadius);
        double width = Width.RelativeTo(clockRadius);
        double halfWidth = width / 2;
        double hipDistance = HipDistance.RelativeTo(clockRadius);
        double tipLength = TipLength.RelativeTo(clockRadius);
        double tipWidth = 0.8.RelativeTo(clockRadius);
        double tipBaseWidth = 2.RelativeTo(clockRadius);
        double baseWidth = 3.RelativeTo(clockRadius);

        // Background

        Point pointA1 = new(-baseWidth / 2, 0);
        Point pointA2 = new(-halfWidth, -hipDistance);
        Point pointA3 = new(-tipBaseWidth / 2, -length + tipLength);
        Point pointA4 = new(-tipWidth / 2, -length);
        Point pointA5 = new(tipWidth / 2, -length);
        Point pointA6 = new(tipBaseWidth / 2, -length + tipLength);
        Point pointA7 = new(halfWidth, -hipDistance);
        Point pointA8 = new(baseWidth / 2, 0);

        StreamGeometry geometry = new();

        StreamGeometryContext streamGeometryContext1 = geometry.Open();

        streamGeometryContext1.BeginFigure(pointA1, true);

        streamGeometryContext1.LineTo(pointA2, true);
        streamGeometryContext1.LineTo(pointA3, true);
        streamGeometryContext1.LineTo(pointA4, true);
        streamGeometryContext1.LineTo(pointA5, true);
        streamGeometryContext1.LineTo(pointA6, true);
        streamGeometryContext1.LineTo(pointA7, true);
        streamGeometryContext1.LineTo(pointA8, true);

        // Background - Finish

        this.geometry = geometry;

        // Stroke Brush

        strokePen = CreateStrokePen();
    }

    protected override void DoRenderHand(ClockDrawingContext context)
    {
        context.DrawingContext.DrawGeometry(FillBrush, strokePen, geometry);
    }
}
