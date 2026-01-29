using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class CapsuleHand : HandBase
{
    #region Width StyledProperty

    public static readonly StyledProperty<double> WidthProperty = AvaloniaProperty.Register<CapsuleHand, double>(
        nameof(Width),
        defaultValue: 4.0);

    public double Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    #endregion

    #region TailLength StyledProperty

    public static readonly StyledProperty<double> TailLengthProperty = AvaloniaProperty.Register<CapsuleHand, double>(
        nameof(TailLength),
        defaultValue: 2.0);

    public double TailLength
    {
        get => GetValue(TailLengthProperty);
        set => SetValue(TailLengthProperty, value);
    }

    #endregion

    static CapsuleHand()
    {
        _ = WidthProperty.Changed.AddClassHandler<CapsuleHand>(HandleWidthChanged);
        _ = TailLengthProperty.Changed.AddClassHandler<CapsuleHand>(HandleTailLengthChanged);
    }

    private static void HandleWidthChanged(CapsuleHand capsuleHand, AvaloniaPropertyChangedEventArgs e)
    {
        capsuleHand.InvalidateLayout();
    }

    private static void HandleTailLengthChanged(CapsuleHand capsuleHand, AvaloniaPropertyChangedEventArgs e)
    {
        capsuleHand.InvalidateLayout();
    }

    private PathGeometry handGeometry;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (Width <= 0)
            return false;

        return base.OnRendering(context);
    }

    protected override void CalculateLayout(ClockDrawingContext context)
    {
        base.CalculateLayout(context);

        handGeometry = CreateHandGeometry(context);
    }

    private PathGeometry CreateHandGeometry(ClockDrawingContext context)
    {
        double radius = context.ClockRadius;
        double handLength = radius * (Length / 100.0);
        double tailLength = radius * (TailLength / 100.0);
        double halfWidth = radius * (Width / 100.0) / 2.0;

        double topY = -handLength + halfWidth;
        double bottomY = tailLength - halfWidth;

        PathFigure capsuleFigure = new()
        {
            StartPoint = new Point(-halfWidth, topY),
            IsClosed = true
        };

        capsuleFigure.Segments ??= [];

        capsuleFigure.Segments.Add(new ArcSegment
        {
            Point = new Point(halfWidth, topY),
            Size = new Size(halfWidth, halfWidth),
            SweepDirection = SweepDirection.Clockwise,
            IsLargeArc = false
        });

        capsuleFigure.Segments.Add(new LineSegment
        {
            Point = new Point(halfWidth, bottomY)
        });

        capsuleFigure.Segments.Add(new ArcSegment
        {
            Point = new Point(-halfWidth, bottomY),
            Size = new Size(halfWidth, halfWidth),
            SweepDirection = SweepDirection.Clockwise,
            IsLargeArc = false
        });

        capsuleFigure.Segments.Add(new LineSegment
        {
            Point = new Point(-halfWidth, topY)
        });

        PathGeometry handGeometry = new();
        handGeometry.Figures.Add(capsuleFigure);

        return handGeometry;
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
                context.DrawingContext.DrawGeometry(FillBrush, StrokePen, handGeometry);
            });
    }
}
