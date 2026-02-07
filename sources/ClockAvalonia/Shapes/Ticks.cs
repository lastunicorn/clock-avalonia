using Avalonia;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Utils;

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
        OrientationProperty.OverrideDefaultValue<Ticks>(RimItemOrientation.FaceIn);
        RoundEndsProperty.Changed.AddClassHandler<Ticks>((ticks, e) => ticks.InvalidateCache());
    }

    private Pen strokePen;

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        strokePen = CreateStrokePen();
    }

    protected override void OnCreateStrokePen(CreateStrokePenEventArgs e)
    {
        base.OnCreateStrokePen(e);

        if (RoundEnds)
            e.StrokePen.LineCap = PenLineCap.Round;
    }

    protected override void RenderItem(ClockDrawingContext context, int index)
    {
        double actualLength = Length.RelativeTo(context.ClockRadius);
        double tipLength = RoundEnds
            ? StrokeThickness / 2
            : 0;

        actualLength -= tipLength * 2;

        Point startPoint = new(0, -actualLength / 2);
        Point endPoint = new(0, actualLength / 2);

        context.DrawingContext.DrawLine(strokePen, startPoint, endPoint);
    }
}
