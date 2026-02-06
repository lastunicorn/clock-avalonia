using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class FancySweepHand : HandBase
{
    #region CircleRadius StyledProperty

    public static readonly StyledProperty<double> CircleRadiusProperty = AvaloniaProperty.Register<FancySweepHand, double>(
        nameof(CircleRadius),
        defaultValue: 7.0);

    public double CircleRadius
    {
        get => GetValue(CircleRadiusProperty);
        set => SetValue(CircleRadiusProperty, value);
    }

    #endregion

    #region CircleOffset StyledProperty

    public static readonly StyledProperty<double> CircleOffsetProperty = AvaloniaProperty.Register<FancySweepHand, double>(
        nameof(CircleOffset),
        defaultValue: 24.0);

    public double CircleOffset
    {
        get => GetValue(CircleOffsetProperty);
        set => SetValue(CircleOffsetProperty, value);
    }

    #endregion

    #region TailLength StyledProperty

    public static readonly StyledProperty<double> TailLengthProperty = AvaloniaProperty.Register<FancySweepHand, double>(
        nameof(TailLength),
        defaultValue: 14.0);

    public double TailLength
    {
        get => GetValue(TailLengthProperty);
        set => SetValue(TailLengthProperty, value);
    }

    #endregion

    static FancySweepHand()
    {
        CircleRadiusProperty.Changed.AddClassHandler<FancySweepHand>((hand, e) => hand.InvalidateCache());
        CircleOffsetProperty.Changed.AddClassHandler<FancySweepHand>((hand, e) => hand.InvalidateCache());
        TailLengthProperty.Changed.AddClassHandler<FancySweepHand>((hand, e) => hand.InvalidateCache());
    }

    private Point mainLineStartPoint;
    private Point mainLineEndPoint;
    private Point circleCenter;
    private double circleRadius;
    private Point tipLineStartPoint;
    private Point tipLineEndPoint;
    private Pen strokePen;

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        double radius = context.ClockRadius;
        double calculatedLength = radius * (Length / 100.0);
        double calculatedCircleOffset = radius * (CircleOffset / 100.0);
        double calculatedCircleRadius = radius * (CircleRadius / 100.0);
        double calculatedTailLength = radius * (TailLength / 100.0);

        double calculatedCircleCenterY = -calculatedLength + calculatedCircleOffset;

        mainLineStartPoint = new Point(0, calculatedTailLength);
        mainLineEndPoint = new Point(0, calculatedCircleCenterY + calculatedCircleRadius);

        circleCenter = new Point(0, calculatedCircleCenterY);
        circleRadius = calculatedCircleRadius;

        tipLineStartPoint = new Point(0, calculatedCircleCenterY - calculatedCircleRadius);
        tipLineEndPoint = new Point(0, -calculatedLength);

        strokePen = CreateStrokePen();
    }

    protected override void DoRenderHand(ClockDrawingContext context)
    {
        context.DrawingContext.DrawLine(strokePen, mainLineStartPoint, mainLineEndPoint);
        context.DrawingContext.DrawEllipse(null, strokePen, circleCenter, circleRadius, circleRadius);
        context.DrawingContext.DrawLine(strokePen, tipLineStartPoint, tipLineEndPoint);
    }
}
