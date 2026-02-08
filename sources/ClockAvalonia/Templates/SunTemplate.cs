using Avalonia;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Templates;

public class SunTemplate : ClockTemplate
{
    protected override IEnumerable<Shape> CreateShapes()
    {
        yield return new FancyBackground
        {
            Name = "Background",
            OuterRimWidth = 14,
            InnerRimWidth = 46,
            OuterRimBrush = CreateOuterRimBrush(),
            InnerRimBrush = CreateInnerRimBrush(),
            FillBrush = CreateFaceBrush()
        };

        yield return new HourNumerals
        {
            Name = "Hour Numerals",
            DistanceFromEdge = 37,
            FontSize = 17,
            FillBrush = Brushes.Black
        };

        yield return new TextRim
        {
            Name = "Minute Numerals",
            Texts = Enumerable.Range(1, 60)
                .Select(x => x.ToString())
                .ToArray(),
            Angle = 6,
            OffsetAngle = 6,
            DistanceFromEdge = 7,
            FontFamily = new FontFamily("Arial"),
            FontSize = 4.4,
            FillBrush = Brushes.Black
        };

        yield return new DotHand
        {
            Name = "Hour Hand",
            TimeComponent = TimeComponent.Hour,
            Length = 63,
            FillBrush = null,
            StrokeBrush = Brushes.Black,
            StrokeThickness = 1,
            Radius = 14,
            IntegralValue = true
        };

        yield return new DotHand
        {
            Name = "Minute Hand",
            TimeComponent = TimeComponent.Minute,
            Length = 93,
            FillBrush = null,
            StrokeBrush = Brushes.Black,
            StrokeThickness = 1,
            Radius = 6,
            IntegralValue = true
        };

        yield return new DotHand
        {
            Name = "Second Hand",
            TimeComponent = TimeComponent.Second,
            Length = 93,
            FillBrush = null,
            StrokeBrush = Brushes.Black,
            StrokeThickness = 0.5,
            Radius = 6
        };
    }

    private static Brush CreateOuterRimBrush()
    {
        LinearGradientBrush brush = new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative)
        };

        brush.GradientStops.Add(new GradientStop(Color.FromRgb(155, 219, 255), 0));
        brush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 64, 128), 1));

        return brush;
    }

    private static Brush CreateInnerRimBrush()
    {
        LinearGradientBrush brush = new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative)
        };

        brush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 64, 128), 0));
        brush.GradientStops.Add(new GradientStop(Color.FromRgb(155, 219, 255), 1));

        return brush;
    }

    private static Brush CreateFaceBrush()
    {
        LinearGradientBrush brush = new()
        {
            StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative)
        };

        brush.GradientStops.Add(new GradientStop(Color.FromRgb(200, 230, 255), 0));
        brush.GradientStops.Add(new GradientStop(Color.FromRgb(50, 100, 150), 1));

        return brush;
    }
}
