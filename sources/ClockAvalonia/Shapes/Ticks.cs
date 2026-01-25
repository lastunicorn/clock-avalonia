using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class Ticks : RimBase
{
    #region Length StyledProperty

    public static readonly StyledProperty<double> LengthProperty = AvaloniaProperty.Register<Ticks, double>(
        nameof(Length),
        defaultValue: 5.0);

    public double Length
    {
        get => GetValue(LengthProperty);
        set => SetValue(LengthProperty, value);
    }

    #endregion

    #region RoundEnds StyledProperty

    public static readonly StyledProperty<bool> RoundEndsProperty = AvaloniaProperty.Register<Ticks, bool>(
        nameof(RoundEnds),
        defaultValue: false);

    public bool RoundEnds
    {
        get => GetValue(RoundEndsProperty);
        set => SetValue(RoundEndsProperty, value);
    }

    #endregion

    static Ticks()
    {
        AngleProperty.OverrideDefaultValue<Ticks>(6.0);
        OffsetAngleProperty.OverrideDefaultValue<Ticks>(6.0);
        DistanceFromEdgeProperty.OverrideDefaultValue<Ticks>(6.0);
        OrientationProperty.OverrideDefaultValue<Ticks>(RimItemOrientation.FaceCenter);
        RoundEndsProperty.Changed.AddClassHandler<Ticks>((ticks, e) => ticks.InvalidateDrawingTools());
    }

    private double radius;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (StrokePen == null)
            return false;

        radius = context.ClockRadius;
        return base.OnRendering(context);
    }

    protected override IPen CreateStrokePen()
    {
        if (StrokeThickness <= 0 || StrokeBrush == null)
            return null;

        PenLineCap lineCap = RoundEnds
            ? PenLineCap.Round
            : PenLineCap.Flat;

        return new Pen(StrokeBrush, StrokeThickness, lineCap: lineCap);
    }

    protected override void RenderItem(DrawingContext drawingContext, int index)
    {
        double actualLength = radius * Length / 100.0;

        Point startPoint = new(0, -actualLength / 2);
        Point endPoint = new(0, actualLength / 2);

        drawingContext.DrawLine(StrokePen, startPoint, endPoint);
    }
}
