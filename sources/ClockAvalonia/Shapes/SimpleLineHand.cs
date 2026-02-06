using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class SimpleLineHand : HandBase
{
    #region TailLength StyledProperty

    public static readonly StyledProperty<double> TailLengthProperty = AvaloniaProperty.Register<SimpleLineHand, double>(
        nameof(TailLength),
        defaultValue: 0.0);

    public double TailLength
    {
        get => GetValue(TailLengthProperty);
        set => SetValue(TailLengthProperty, value);
    }

    #endregion

    #region PinDiameter StyledProperty

    public static readonly StyledProperty<double> PinDiameterProperty = AvaloniaProperty.Register<SimpleLineHand, double>(
        nameof(PinDiameter),
        defaultValue: 4.0);

    public double PinDiameter
    {
        get => GetValue(PinDiameterProperty);
        set => SetValue(PinDiameterProperty, value);
    }

    #endregion

    protected override void OnCreateStrokePen(CreateStrokePenEventArgs e)
    {
        base.OnCreateStrokePen(e);

        e.StrokePen.LineCap = PenLineCap.Round;
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
                double radius = context.ClockDiameter / 2;

                DrawHandLine(dc, radius);
                DrawPin(dc, radius);
            });
    }

    private void DrawHandLine(DrawingContext drawingContext, double radius)
    {
        Pen strokePen = CreateStrokePen();

        if (strokePen == null)
            return;

        if (Length <= 0 && TailLength <= 0)
            return;

        double handLength = radius * (Length / 100.0);
        double tailLength = radius * (TailLength / 100.0);

        Point startPoint = new(0, tailLength);
        Point endPoint = new(0, -handLength);

        drawingContext.DrawLine(strokePen, startPoint, endPoint);
    }

    private void DrawPin(DrawingContext drawingContext, double radius)
    {
        if (StrokeBrush == null)
            return;

        if (PinDiameter <= 0)
            return;

        double calculatedPinDiameter = radius * (PinDiameter / 100.0);
        double calculatedPinRadius = calculatedPinDiameter / 2;

        Point center = new(0, 0);
        drawingContext.DrawEllipse(StrokeBrush, null, center, calculatedPinRadius, calculatedPinRadius);
    }
}
