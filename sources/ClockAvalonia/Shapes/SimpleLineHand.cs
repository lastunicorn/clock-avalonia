using System.Net;
using Avalonia;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Utils;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class SimpleLineHand : HandBase
{
    #region RoundEnds StyledProperty

    public static readonly StyledProperty<bool> RoundEndsProperty = AvaloniaProperty.Register<SimpleLineHand, bool>(
        nameof(RoundEnds),
        defaultValue: false);

    public bool RoundEnds
    {
        get => GetValue(RoundEndsProperty);
        set => SetValue(RoundEndsProperty, value);
    }

    #endregion

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

    static SimpleLineHand()
    {
        _ = RoundEndsProperty.Changed.AddClassHandler<SimpleLineHand>((hand, e) => hand.HandleRoundEndsChanged(e));
        _ = TailLengthProperty.Changed.AddClassHandler<SimpleLineHand>((hand, e) => hand.HandleTailLengthChanged(e));
    }

    private void HandleRoundEndsChanged(AvaloniaPropertyChangedEventArgs e)
    {
        InvalidateCache();
        OnChanged(EventArgs.Empty);
    }

    private void HandleTailLengthChanged(AvaloniaPropertyChangedEventArgs e)
    {
        InvalidateCache();
        OnChanged(EventArgs.Empty);
    }

    private Point startPoint;
    private Point endPoint;
    private Pen strokePen;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (StrokeThickness <= 0 || StrokeBrush == null)
            return false;

        return base.OnRendering(context);
    }

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        // Hand

        double radius = context.ClockRadius;
        double calculatedLength = Length.RelativeTo(radius);
        double calculatedTailLength = TailLength.RelativeTo(radius);
        double calculatedTipLength = RoundEnds
            ? StrokeThickness / 2
            : 0;

        startPoint = new(0, calculatedTailLength - calculatedTipLength);
        endPoint = new(0, -calculatedLength + calculatedTipLength);

        strokePen = CreateStrokePen();
    }

    protected override void OnCreateStrokePen(CreateStrokePenEventArgs e)
    {
        base.OnCreateStrokePen(e);

        if (RoundEnds)
            e.StrokePen.LineCap = PenLineCap.Round;
    }

    protected override void DoRenderHand(ClockDrawingContext context)
    {
        context.DrawingContext.DrawLine(strokePen, startPoint, endPoint);
    }
}
