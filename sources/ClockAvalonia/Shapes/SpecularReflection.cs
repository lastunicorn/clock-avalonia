using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class SpecularReflection : Shape
{
    static SpecularReflection()
    {
        FillBrushProperty.OverrideDefaultValue<SpecularReflection>(Brushes.WhiteSmoke);
        StrokeThicknessProperty.OverrideDefaultValue<SpecularReflection>(0.0);
    }

    private Pen strokePen;

    public SpecularReflection()
    {
        RadialGradientBrush brush = new()
        {
            GradientOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative)
        };

        brush.GradientStops.Add(new GradientStop(Color.FromArgb(0xa0, 0xff, 0xff, 0xff), 0));
        brush.GradientStops.Add(new GradientStop(Color.FromArgb(0xa0, 0xff, 0xff, 0xff), 0.9));
        brush.GradientStops.Add(new GradientStop(Color.FromArgb(0x10, 0xff, 0xff, 0xff), 1));

        FillBrush = brush;
    }

    protected override void CalculateCache(ClockDrawingContext context)
    {
        base.CalculateCache(context);

        strokePen = CreateStrokePen();
    }

    protected override void DoRender(ClockDrawingContext context)
    {
        double x = -context.ClockRadius + (context.ClockRadius * 50) / 100 - 10;
        double y = -context.ClockRadius + (context.ClockRadius * 50) / 100 + 20;

        Point center = new(x, y);
        double radiusX = (context.ClockRadius * 35) / 100;
        double radiusY = (context.ClockRadius * 15) / 100;

        Matrix rotation = Matrix.CreateRotation(Math.PI * -65 / 180);
        Matrix translation = Matrix.CreateTranslation(center.X, center.Y);
        Matrix transform = rotation * translation * Matrix.CreateTranslation(-center.X, -center.Y) * Matrix.CreateTranslation(center.X, center.Y);

        using (context.DrawingContext.PushTransform(Matrix.CreateTranslation(-center.X, -center.Y)))
        using (context.DrawingContext.PushTransform(rotation))
        using (context.DrawingContext.PushTransform(Matrix.CreateTranslation(center.X, center.Y)))
        {
            context.DrawingContext.DrawEllipse(FillBrush, strokePen, center, radiusX, radiusY);
        }
    }
}
