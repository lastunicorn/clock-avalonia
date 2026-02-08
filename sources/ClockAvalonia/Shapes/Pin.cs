using Avalonia;
using Avalonia.Media;

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
        DiameterProperty.Changed.AddClassHandler<Pin>((pin, e) => pin.InvalidateCache());
        StrokeThicknessProperty.OverrideDefaultValue<Pin>(0.0);
    }

    private Point pinCenter;
    private double pinRadius;
    private Pen strokePen;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (Diameter <= 0)
            return false;

        return base.OnRendering(context);
    }

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        pinRadius = context.ClockRadius * (Diameter / 100.0) / 2;
        pinCenter = new Point(0, 0);

        strokePen = CreateStrokePen();
    }

    protected override void DoRender(ClockDrawingContext context)
    {
        context.DrawingContext.DrawEllipse(FillBrush, strokePen, pinCenter, pinRadius, pinRadius);
    }
}
