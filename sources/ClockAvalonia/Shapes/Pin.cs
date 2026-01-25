using Avalonia;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class Pin : Shape
{
    #region Diameter StyledProperty

    public static readonly StyledProperty<double> DiameterProperty = AvaloniaProperty.Register<Pin, double>(
        nameof(Diameter),
        defaultValue: 4.0);

    public double Diameter
    {
        get => GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    #endregion

    static Pin()
    {
        DiameterProperty.Changed.AddClassHandler<Pin>((pin, e) => pin.InvalidateLayout());
    }

    private Point pinCenter;
    private double pinRadius;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (FillBrush == null && StrokePen == null)
            return false;

        if (Diameter <= 0)
            return false;

        return base.OnRendering(context);
    }

    protected override void CalculateLayout(ClockDrawingContext context)
    {
        base.CalculateLayout(context);

        pinRadius = context.ClockRadius * (Diameter / 100.0) / 2;
        pinCenter = new Point(0, 0);
    }

    public override void DoRender(ClockDrawingContext context)
    {
        context.DrawingContext.DrawEllipse(FillBrush, StrokePen, pinCenter, pinRadius, pinRadius);
    }
}
