using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class RodHand : HandBase
{
    #region Width StyledProperty

    public static readonly StyledProperty<double> WidthProperty = AvaloniaProperty.Register<RodHand, double>(
        nameof(Width),
        defaultValue: 4.0);

    public double Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    #endregion

    #region TailLength StyledProperty

    public static readonly StyledProperty<double> TailLengthProperty = AvaloniaProperty.Register<RodHand, double>(
        nameof(TailLength),
        defaultValue: 2.0);

    public double TailLength
    {
        get => GetValue(TailLengthProperty);
        set => SetValue(TailLengthProperty, value);
    }

    #endregion

    static RodHand()
    {
        _ = WidthProperty.Changed.AddClassHandler<RodHand>(HandleWidthChanged);
        _ = TailLengthProperty.Changed.AddClassHandler<RodHand>(HandleTailLengthChanged);
    }

    private static void HandleWidthChanged(RodHand capsuleHand, AvaloniaPropertyChangedEventArgs e)
    {
        capsuleHand.InvalidateCache();
        capsuleHand.OnChanged(EventArgs.Empty);
    }

    private static void HandleTailLengthChanged(RodHand capsuleHand, AvaloniaPropertyChangedEventArgs e)
    {
        capsuleHand.InvalidateCache();
        capsuleHand.OnChanged(EventArgs.Empty);
    }

    private PathGeometry handGeometry;
    private Pen strokePen;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (Width <= 0)
            return false;

        return base.OnRendering(context);
    }

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        handGeometry = CreateHandGeometry(context);
        strokePen = CreateStrokePen();
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

    protected override void DoRenderHand(ClockDrawingContext context)
    {
        context.DrawingContext.DrawGeometry(FillBrush, strokePen, handGeometry);
    }
}
