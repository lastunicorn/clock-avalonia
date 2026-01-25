using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class NibHand : HandBase
{
    #region Width StyledProperty

    public static readonly StyledProperty<double> WidthProperty = AvaloniaProperty.Register<NibHand, double>(
        nameof(Width),
        defaultValue: 5.0);

    public double Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    #endregion

    #region KeepProportions StyledProperty

    public static readonly StyledProperty<bool> KeepProportionsProperty = AvaloniaProperty.Register<NibHand, bool>(
        nameof(KeepProportions),
        defaultValue: true);

    public bool KeepProportions
    {
        get => GetValue(KeepProportionsProperty);
        set => SetValue(KeepProportionsProperty, value);
    }

    #endregion

    private PathGeometry nibGeometry;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (FillBrush == null && StrokePen == null)
            return false;

        return base.OnRendering(context);
    }

    protected override void CalculateLayout(ClockDrawingContext context)
    {
        base.CalculateLayout(context);
        nibGeometry = CreateNibGeometry();
    }

    private static PathGeometry CreateNibGeometry()
    {
        PathFigure figure = new()
        {
            IsClosed = true
        };

        figure.Segments ??= [];

        double startAngle = -60;
        double centerY = 43 + 12;
        double startX = 12 * Math.Cos(startAngle * Math.PI / 180);
        double startY = centerY + 12 * Math.Sin(startAngle * Math.PI / 180);

        figure.StartPoint = new Point(startX, startY);

        double endAngle = startAngle + 300;
        double endX = 12 * Math.Cos(endAngle * Math.PI / 180);
        double endY = centerY + 12 * Math.Sin(endAngle * Math.PI / 180);

        figure.Segments.Add(new ArcSegment
        {
            Point = new Point(endX, endY),
            Size = new Size(12, 12),
            IsLargeArc = true,
            SweepDirection = SweepDirection.Clockwise
        });

        figure.Segments.Add(new BezierSegment
        {
            Point1 = new Point(-6.67, 42.67),
            Point2 = new Point(-5.33, 40.33),
            Point3 = new Point(-8, 35)
        });

        figure.Segments.Add(new BezierSegment
        {
            Point1 = new Point(-6.67, 32.33),
            Point2 = new Point(-4.67, 26.67),
            Point3 = new Point(-2, 11)
        });

        figure.Segments.Add(new ArcSegment
        {
            Point = new Point(-12, -1),
            Size = new Size(12, 12),
            IsLargeArc = false,
            SweepDirection = SweepDirection.Clockwise
        });

        figure.Segments.Add(new BezierSegment
        {
            Point1 = new Point(-9.33, -24.33),
            Point2 = new Point(-4.67, -45.67),
            Point3 = new Point(-10, -119)
        });

        figure.Segments.Add(new BezierSegment
        {
            Point1 = new Point(-8.33, -121),
            Point2 = new Point(-6, -125),
            Point3 = new Point(-3, -129)
        });

        figure.Segments.Add(new ArcSegment
        {
            Point = new Point(-15, -144),
            Size = new Size(15, 15),
            IsLargeArc = false,
            SweepDirection = SweepDirection.Clockwise
        });

        figure.Segments.Add(new BezierSegment
        {
            Point1 = new Point(-11, -163.67),
            Point2 = new Point(-5.67, -191.33),
            Point3 = new Point(-1, -249)
        });

        figure.Segments.Add(new LineSegment { Point = new Point(-1, -280) });

        figure.Segments.Add(new LineSegment { Point = new Point(1, -280) });

        figure.Segments.Add(new LineSegment { Point = new Point(1, -249) });

        figure.Segments.Add(new BezierSegment
        {
            Point1 = new Point(5.67, -191.33),
            Point2 = new Point(11, -163.67),
            Point3 = new Point(15, -144)
        });

        figure.Segments.Add(new ArcSegment
        {
            Point = new Point(0, -129),
            Size = new Size(15, 15),
            IsLargeArc = false,
            SweepDirection = SweepDirection.Clockwise
        });

        figure.Segments.Add(new BezierSegment
        {
            Point1 = new Point(6, -125),
            Point2 = new Point(8.33, -121),
            Point3 = new Point(10, -119)
        });

        figure.Segments.Add(new BezierSegment
        {
            Point1 = new Point(4.67, -45.67),
            Point2 = new Point(9.33, -24.33),
            Point3 = new Point(12, -1)
        });

        figure.Segments.Add(new ArcSegment
        {
            Point = new Point(0, 11),
            Size = new Size(12, 12),
            IsLargeArc = false,
            SweepDirection = SweepDirection.Clockwise
        });

        figure.Segments.Add(new BezierSegment
        {
            Point1 = new Point(4.67, 26.67),
            Point2 = new Point(6.67, 32.33),
            Point3 = new Point(8, 35)
        });

        double finalX = 10 * Math.Cos(Math.PI / 3.0);
        double finalY = 41 * Math.Sin(Math.PI / 3.0);
        figure.Segments.Add(new BezierSegment
        {
            Point1 = new Point(5.33, 40.33),
            Point2 = new Point(6.67, 42.67),
            Point3 = new Point(finalX, finalY)
        });

        PathGeometry geometry = new();
        geometry.Figures.Add(figure);

        return geometry;
    }

    public override void DoRender(ClockDrawingContext context)
    {
        DrawingPlan.Create(context.DrawingContext)
            .WithTransform(() =>
            {
                double angleDegrees = CalculateHandAngle(context.Time);
                return new RotateTransform(angleDegrees);
            })
            .WithTransform(() =>
            {
                double scaleFactorY = Length > 0
                    ? Length / 280.0
                    : 1.0;

                double scaleFactorX = KeepProportions
                    ? scaleFactorY
                    : (Width > 0 ? Width / 30.0 : 1.0);

                return new ScaleTransform(scaleFactorX, scaleFactorY);
            })
            .Draw(dc =>
            {
                dc.DrawGeometry(FillBrush, StrokePen, nibGeometry);
            });
    }
}
