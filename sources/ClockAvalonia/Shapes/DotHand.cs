using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class DotHand : HandBase
{
    #region Radius StyledProperty

    public static readonly StyledProperty<double> RadiusProperty = AvaloniaProperty.Register<DotHand, double>(
        nameof(Radius),
        defaultValue: 5.0);

    public double Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    #endregion

    private double actualRadius;
    private Point center;
    private Pen strokePen;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (Radius <= 0)
            return false;

        return base.OnRendering(context);
    }

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        double clockRadius = context.ClockRadius;
        double actualLength = clockRadius * (Length / 100.0);
        actualRadius = clockRadius * (Radius / 100.0);

        center = new Point(0, -actualLength);

        strokePen = CreateStrokePen();
    }

    protected override void DoRenderHand(ClockDrawingContext context)
    {
        context.DrawingContext.DrawEllipse(FillBrush, strokePen, center, actualRadius, actualRadius);
    }
}
