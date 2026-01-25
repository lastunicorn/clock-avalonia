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

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (FillBrush == null && StrokePen == null)
            return false;

        return base.OnRendering(context);
    }

    public override void DoRender(ClockDrawingContext context)
    {
        Point center = new Point(0, 0);
        double backgroundRadius = (context.ClockDiameter - StrokeThickness) / 2;

        context.DrawingContext.DrawEllipse(FillBrush, StrokePen, center, backgroundRadius, backgroundRadius);
    }
}
