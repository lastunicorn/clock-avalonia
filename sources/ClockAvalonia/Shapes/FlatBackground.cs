using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class FlatBackground : Shape
{
    static FlatBackground()
    {
        FillBrushProperty.OverrideDefaultValue<FlatBackground>(Brushes.WhiteSmoke);
        StrokeThicknessProperty.OverrideDefaultValue<FlatBackground>(0.0);
    }

    private Pen strokePen;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (FillBrush == null && strokePen == null)
            return false;

        return base.OnRendering(context);
    }

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        strokePen = CreateStrokePen();
    }

    protected override void DoRender(ClockDrawingContext context)
    {
        Point center = new(0, 0);
        double backgroundRadius = (context.ClockDiameter - StrokeThickness) / 2;

        context.DrawingContext.DrawEllipse(FillBrush, strokePen, center, backgroundRadius, backgroundRadius);
    }
}
