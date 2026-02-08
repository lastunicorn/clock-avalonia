using Avalonia;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Utils;
using DustInTheWind.ClockWpf.Utils;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class FancyBackground : Shape
{
    #region OuterRimWidth StyledProperty

    public static readonly StyledProperty<double> OuterRimWidthProperty = AvaloniaProperty.Register<FancyBackground, double>(
        nameof(OuterRimWidth),
        defaultValue: 10.0);

    public double OuterRimWidth
    {
        get => GetValue(OuterRimWidthProperty);
        set => SetValue(OuterRimWidthProperty, value);
    }

    #endregion

    #region InnerRimWidth StyledProperty

    public static readonly StyledProperty<double> InnerRimWidthProperty = AvaloniaProperty.Register<FancyBackground, double>(
        nameof(InnerRimWidth),
        defaultValue: 2.0);

    public double InnerRimWidth
    {
        get => GetValue(InnerRimWidthProperty);
        set => SetValue(InnerRimWidthProperty, value);
    }

    #endregion

    #region OuterRimBrush StyledProperty

    public static readonly StyledProperty<IBrush> OuterRimBrushProperty = AvaloniaProperty.Register<FancyBackground, IBrush>(
        nameof(OuterRimBrush));

    public IBrush OuterRimBrush
    {
        get => GetValue(OuterRimBrushProperty);
        set => SetValue(OuterRimBrushProperty, value);
    }

    #endregion

    #region InnerRimBrush StyledProperty

    public static readonly StyledProperty<IBrush> InnerRimBrushProperty = AvaloniaProperty.Register<FancyBackground, IBrush>(
        nameof(InnerRimBrush));

    public IBrush InnerRimBrush
    {
        get => GetValue(InnerRimBrushProperty);
        set => SetValue(InnerRimBrushProperty, value);
    }

    #endregion

    #region FillColor StyledProperty

    public static readonly StyledProperty<Color> FillColorProperty = AvaloniaProperty.Register<FancyBackground, Color>(
        nameof(FillColor),
        Colors.Black);

    public Color FillColor
    {
        get => GetValue(FillColorProperty);
        set => SetValue(FillColorProperty, value);
    }

    #endregion

    static FancyBackground()
    {
        FillBrushProperty.OverrideDefaultValue<FancyBackground>((Brush)null);
        StrokeThicknessProperty.OverrideDefaultValue<FancyBackground>(0.0);

        OuterRimWidthProperty.Changed.AddClassHandler<FancyBackground>(HandleOuterRimWidthChanged);
        InnerRimWidthProperty.Changed.AddClassHandler<FancyBackground>(HandleInnerRimWidthChanged);
        OuterRimBrushProperty.Changed.AddClassHandler<FancyBackground>(HandleOuterRimBrushChanged);
        InnerRimBrushProperty.Changed.AddClassHandler<FancyBackground>(HandleInnerRimBrushChanged);
        FillColorProperty.Changed.AddClassHandler<FancyBackground>(HandleFillColorChanged);
    }

    private static void HandleOuterRimWidthChanged(FancyBackground fancyBackground, AvaloniaPropertyChangedEventArgs e)
    {
        fancyBackground.InvalidateCache();
        fancyBackground.OnChanged(EventArgs.Empty);
    }

    private static void HandleInnerRimWidthChanged(FancyBackground fancyBackground, AvaloniaPropertyChangedEventArgs e)
    {
        fancyBackground.InvalidateCache();
        fancyBackground.OnChanged(EventArgs.Empty);
    }

    private static void HandleOuterRimBrushChanged(FancyBackground fancyBackground, AvaloniaPropertyChangedEventArgs e)
    {
        fancyBackground.generateBrushesFromColor = false;
        fancyBackground.InvalidateCache();
        fancyBackground.OnChanged(EventArgs.Empty);
    }

    private static void HandleInnerRimBrushChanged(FancyBackground fancyBackground, AvaloniaPropertyChangedEventArgs e)
    {
        fancyBackground.generateBrushesFromColor = false;
        fancyBackground.InvalidateCache();
        fancyBackground.OnChanged(EventArgs.Empty);
    }

    private static void HandleFillColorChanged(FancyBackground fancyBackground, AvaloniaPropertyChangedEventArgs e)
    {
        fancyBackground.generateBrushesFromColor = false;
        fancyBackground.InvalidateCache();
        fancyBackground.OnChanged(EventArgs.Empty);
    }

    private bool generateBrushesFromColor = true;
    private double calculatedOuterRimRadius;
    private double calculatedInnerRimRadius;
    private double calculatedFaceRadius;
    private IBrush calculatedOuterRimBrush;
    private IBrush calculatedInnerRimBrush;
    private IBrush calculatedFaceBrush;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        return true;
    }

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        double clockRadius = context.ClockRadius;

        calculatedOuterRimRadius = clockRadius;

        double calculatedOuterRimWidth = OuterRimWidth.RelativeTo(clockRadius);
        calculatedInnerRimRadius = clockRadius - calculatedOuterRimWidth;

        double calculatedInnerRimWidth = InnerRimWidth.RelativeTo(clockRadius);
        calculatedFaceRadius = calculatedInnerRimRadius - calculatedInnerRimWidth;

        if (generateBrushesFromColor)
        {
            calculatedOuterRimBrush = CreateDefaultOuterRimBrush(FillColor);
            calculatedInnerRimBrush = CreateDefaultInnerRimBrush(FillColor);
            calculatedFaceBrush = CreateDefaultFaceBrush(FillColor);
        }
        else
        {
            calculatedOuterRimBrush = OuterRimBrush;
            calculatedInnerRimBrush = InnerRimBrush;
            calculatedFaceBrush = FillBrush;
        }
    }

    private static IBrush CreateDefaultOuterRimBrush(Color color)
    {
        LinearGradientBrush brush = new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative)
        };

        brush.GradientStops.Add(new GradientStop(color.ShiftBrighness(100f), 0));
        brush.GradientStops.Add(new GradientStop(color.ShiftBrighness(-100f), 1));

        return brush;
    }

    private static IBrush CreateDefaultInnerRimBrush(Color color)
    {
        LinearGradientBrush brush = new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative)
        };

        brush.GradientStops.Add(new GradientStop(color.ShiftBrighness(-100f), 0));
        brush.GradientStops.Add(new GradientStop(color.ShiftBrighness(100f), 1));

        return brush;
    }

    private static IBrush CreateDefaultFaceBrush(Color color)
    {
        LinearGradientBrush brush = new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative)
        };

        brush.GradientStops.Add(new GradientStop(color.ShiftBrighness(100f), 0));
        brush.GradientStops.Add(new GradientStop(color.ShiftBrighness(-100f), 1));

        return brush;
    }

    protected override void DoRender(ClockDrawingContext context)
    {
        Point center = new(0, 0);

        context.DrawingContext.DrawEllipse(calculatedOuterRimBrush, null, center, calculatedOuterRimRadius, calculatedOuterRimRadius);
        context.DrawingContext.DrawEllipse(calculatedInnerRimBrush, null, center, calculatedInnerRimRadius, calculatedInnerRimRadius);
        context.DrawingContext.DrawEllipse(calculatedFaceBrush, null, center, calculatedFaceRadius, calculatedFaceRadius);
    }
}
