using Avalonia;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Templates;

/// <summary>
/// Provides a fancy visual template for rendering a clock, featuring nib-style hands and a decorative sweep hand.
/// </summary>
public class FancyTemplate : ClockTemplate
{
    protected override IEnumerable<Shape> CreateShapes()
    {
        GradientStops gradientStops =
        [
            new GradientStop(Colors.WhiteSmoke, 0),
            new GradientStop(Colors.WhiteSmoke, 0.5),
            new GradientStop(Colors.LightGray, 1)
        ];

        LinearGradientBrush linearGradientBrush = new()
        {
            StartPoint = new RelativePoint(0.25, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.75, 1, RelativeUnit.Relative),
            GradientStops = gradientStops
        };

        yield return new FlatBackground
        {
            Name = "Background",
            FillBrush = linearGradientBrush
        };

        yield return new Ticks
        {
            Name = "Minute Ticks",
            StrokeBrush = Brushes.Black,
            Length = 6,
            StrokeThickness = 0.3,
            DistanceFromEdge = 8,
            Angle = 6,
            OffsetAngle = 6,
            SkipIndex = 5
        };

        yield return new Ticks
        {
            Name = "Hour Ticks",
            StrokeBrush = Brushes.Black,
            Length = 6,
            StrokeThickness = 1,
            DistanceFromEdge = 8,
            Angle = 30,
            OffsetAngle = 30
        };

        yield return new HourNumerals
        {
            Name = "Hour Numerals",
            DistanceFromEdge = 28,
            FillBrush = Brushes.Black,
            FontSize = 20
        };

        yield return new NibHand
        {
            Name = "Hour Hand",
            FillBrush = Brushes.Black,
            Length = 60,
            TimeComponent = TimeComponent.Hour,
            Width = 10,
            StrokeBrush = new SolidColorBrush(Color.FromRgb(0x60, 0x60, 0x60)),
            StrokeThickness = 1.5
        };

        yield return new NibHand
        {
            Name = "Minute Hand",
            FillBrush = Brushes.Black,
            TimeComponent = TimeComponent.Minute,
            StrokeBrush = new SolidColorBrush(Color.FromRgb(0x60, 0x60, 0x60)),
            StrokeThickness = 1.5,
            Length = 86
        };

        yield return new FancySweepHand
        {
            Name = "Second Hand",
            TimeComponent = TimeComponent.Second,
            Length = 86,
            StrokeBrush = Brushes.Red,
            StrokeThickness = 0.5
        };

        yield return new Pin
        {
            Name = "Pin",
            FillBrush = new SolidColorBrush(Color.FromRgb(0x64, 0x64, 0x64)),
            StrokeThickness = 0,
            Diameter = 4
        };
    }
}
