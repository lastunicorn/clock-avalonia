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

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (Radius <= 0)
            return false;

        return base.OnRendering(context);
    }

    protected override void CalculateLayout(ClockDrawingContext context)
    {
        base.CalculateLayout(context);

        double clockRadius = context.ClockRadius;
        double actualLength = clockRadius * (Length / 100.0);
        actualRadius = clockRadius * (Radius / 100.0);

        center = new Point(0, -actualLength);
    }

    public override void DoRender(ClockDrawingContext context)
    {
        DrawingPlan.Create(context.DrawingContext)
            .WithTransform(() =>
            {
                double angleDegrees = CalculateHandAngle(context.Time);
                return new RotateTransform(angleDegrees);
            })
            .Draw(dc =>
            {
                dc.DrawEllipse(FillBrush, StrokePen, center, actualRadius, actualRadius);
            });
    }
}
