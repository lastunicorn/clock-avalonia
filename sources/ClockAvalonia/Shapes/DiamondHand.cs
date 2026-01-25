using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class DiamondHand : HandBase
{
    #region Width StyledProperty

    public static readonly StyledProperty<double> WidthProperty = AvaloniaProperty.Register<DiamondHand, double>(
        nameof(Width),
        defaultValue: 5.0);

    public double Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    #endregion

    #region TailLength StyledProperty

    public static readonly StyledProperty<double> TailLengthProperty = AvaloniaProperty.Register<DiamondHand, double>(
        nameof(TailLength),
        defaultValue: 6.0);

    public double TailLength
    {
        get => GetValue(TailLengthProperty);
        set => SetValue(TailLengthProperty, value);
    }

    #endregion

    static DiamondHand()
    {
        WidthProperty.Changed.AddClassHandler<DiamondHand>((hand, e) => hand.InvalidateLayout());
        TailLengthProperty.Changed.AddClassHandler<DiamondHand>((hand, e) => hand.InvalidateLayout());
    }

    private StreamGeometry? diamondGeometry;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (Width <= 0)
            return false;

        return base.OnRendering(context);
    }

    protected override void CalculateLayout(ClockDrawingContext context)
    {
        base.CalculateLayout(context);

        diamondGeometry = CreateDiamondGeometry(context);
    }

    private StreamGeometry CreateDiamondGeometry(ClockDrawingContext context)
    {
        double radius = context.ClockRadius;
        double handLength = radius * (Length / 100.0);
        double tailLength = radius * (TailLength / 100.0);
        double halfWidth = radius * (Width / 100.0) / 2.0;

        StreamGeometry geometry = new();
        using (StreamGeometryContext ctx = geometry.Open())
        {
            ctx.BeginFigure(new Point(0, tailLength), true);
            ctx.LineTo(new Point(-halfWidth, 0));
            ctx.LineTo(new Point(0, -handLength));
            ctx.LineTo(new Point(halfWidth, 0));
            ctx.EndFigure(true);
        }

        return geometry;
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
                context.DrawingContext.DrawGeometry(FillBrush, StrokePen, diamondGeometry);
            });
    }
}
