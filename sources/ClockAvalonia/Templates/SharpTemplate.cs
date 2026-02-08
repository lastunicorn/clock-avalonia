using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Templates;

/// <summary>
/// Provides a predefined clock template with a black-themed design, including default background, angular, and hand
/// shapes.
/// </summary>
/// <remarks>Use this class to create a clock template with a standard set of shapes and styles suitable
/// for a black or dark-themed clock face. The template initializes the BackgroundShapes, AngularShapes, and
/// HandShapes properties with default values representing a complete analog clock layout.</remarks>
public class SharpTemplate : ClockTemplate
{
    protected override IEnumerable<Shape> CreateShapes()
    {
        yield return new FancyBackground
        {
            Name = "Fancy Background",
            FillColor = Colors.Black
        };

        yield return new Ticks
        {
            Name = "Minute Ticks",
            DistanceFromEdge = 16.5,
            Angle = 6,
            OffsetAngle = 6,
            SkipIndex = 5,
            StrokeThickness = 0.3
        };

        yield return new Ticks
        {
            Name = "Hour Ticks",
            DistanceFromEdge = 16.5,
            Angle = 30,
            OffsetAngle = 30,
            StrokeBrush = Brushes.White,
            StrokeThickness = 1
        };

        yield return new HourNumerals
        {
            Name = "Hour Numerals",
            DistanceFromEdge = 32,
            FillBrush = Brushes.LightGray,
            FontFamily = new FontFamily("Vivaldi"),
            FontSize = 16,
            FontWeight = FontWeight.Normal
        };

        yield return new TextRim
        {
            Name = "Minute Numerals",
            Angle = 30,
            OffsetAngle = 30,
            DistanceFromEdge = 5.5,
            FillBrush = Brushes.DarkGray,
            FontFamily = new FontFamily("Arial"),
            FontSize = 5.5,
            Texts = Enumerable.Range(1, 12)
                .Select(x => (x * 5).ToString())
                .ToArray()
        };

        yield return new DiamondHand
        {
            Name = "Hour Hand",
            TimeComponent = TimeComponent.Hour,
            FillBrush = Brushes.RoyalBlue,
            Length = 50,
            Width = 10,
            TailLength = 8,
            StrokeThickness = 0
        };

        yield return new DiamondHand
        {
            Name = "Minute Hand",
            TimeComponent = TimeComponent.Minute,
            FillBrush = Brushes.LimeGreen,
            Length = 76,
            Width = 8,
            TailLength = 8,
            StrokeThickness = 0
        };

        yield return new SimpleLineHand
        {
            Name = "Second Hand",
            TimeComponent = TimeComponent.Second,
            Length = 86,
            TailLength = 14,
            StrokeBrush = Brushes.Red,
            StrokeThickness = 0.3
        };

        yield return new Pin()
        {
            Name = "Pin",
            Diameter = 2,
            FillBrush = Brushes.Red
        };
    }
}
