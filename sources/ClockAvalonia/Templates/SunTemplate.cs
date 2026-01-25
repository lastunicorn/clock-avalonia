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
            InnerRimWidth = 46
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

        yield return new TextRim
        {
            Name = "Hour Numerals",
            Texts = Enumerable.Range(1, 12)
                .Select(x => x.ToString())
                .ToArray(),
            Angle = 30,
            OffsetAngle = 30,
            DistanceFromEdge = 37,
            FontFamily = new FontFamily("Arial"),
            FontSize = 17,
            Orientation = RimItemOrientation.Normal,
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
}
