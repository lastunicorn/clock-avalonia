using Avalonia;
using Avalonia.Media;

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
        nameof(OuterRimBrush),
        defaultValue: CreateDefaultOuterRimBrush());

    private static IBrush CreateDefaultOuterRimBrush()
    {
        LinearGradientBrush brush = new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative)
        };

        brush.GradientStops.Add(new GradientStop(Color.FromRgb(155, 219, 255), 0));
        brush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 64, 128), 1));

        return brush;
    }

    public IBrush OuterRimBrush
    {
        get => GetValue(OuterRimBrushProperty);
        set => SetValue(OuterRimBrushProperty, value);
    }

    #endregion

    #region InnerRimBrush StyledProperty

    public static readonly StyledProperty<IBrush?> InnerRimBrushProperty = AvaloniaProperty.Register<FancyBackground, IBrush>(
        nameof(InnerRimBrush),
        defaultValue: CreateDefaultInnerRimBrush());

    private static IBrush CreateDefaultInnerRimBrush()
    {
        LinearGradientBrush brush = new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative)
        };

        brush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 64, 128), 0));
        brush.GradientStops.Add(new GradientStop(Color.FromRgb(155, 219, 255), 1));

        return brush;
    }

    public IBrush InnerRimBrush
    {
        get => GetValue(InnerRimBrushProperty);
        set => SetValue(InnerRimBrushProperty, value);
    }

    #endregion

    static FancyBackground()
    {
        FillBrushProperty.OverrideDefaultValue<FancyBackground>(CreateDefaultFaceBrush());
        StrokeThicknessProperty.OverrideDefaultValue<FancyBackground>(0.0);
    }

    private static IBrush CreateDefaultFaceBrush()
    {
        LinearGradientBrush brush = new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative)
        };

        brush.GradientStops.Add(new GradientStop(Color.FromRgb(200, 230, 255), 0));
        brush.GradientStops.Add(new GradientStop(Color.FromRgb(50, 100, 150), 1));

        return brush;
    }

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (OuterRimBrush == null && InnerRimBrush == null && FillBrush == null)
            return false;

        return base.OnRendering(context);
    }

    public override void DoRender(ClockDrawingContext context)
    {
        Point center = new(0, 0);
        double clockRadius = context.ClockRadius;

        double calculatedOuterRimWidth = clockRadius * (OuterRimWidth / 100);
        double innerRimRadius = clockRadius - calculatedOuterRimWidth;

        double calculatedInnerRimWidth = clockRadius * (InnerRimWidth / 100);
        double faceRadius = innerRimRadius - calculatedInnerRimWidth;

        if (OuterRimBrush != null)
            context.DrawingContext.DrawEllipse(OuterRimBrush, null, center, clockRadius, clockRadius);

        if (InnerRimBrush != null)
            context.DrawingContext.DrawEllipse(InnerRimBrush, null, center, innerRimRadius, innerRimRadius);

        if (FillBrush != null)
            context.DrawingContext.DrawEllipse(FillBrush, StrokePen, center, faceRadius, faceRadius);
    }
}
