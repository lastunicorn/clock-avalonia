using Avalonia;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Templates;

public class Blade2Template : ClockTemplate
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

        yield return new HourNumerals
        {
            Name = "Hour Numerals",
            Texts = ["3", "6", "9", "12"],
            Angle = 90,
            OffsetAngle = 90,
            FillBrush = Brushes.Black,
            DistanceFromEdge = 18
        };

        yield return new Ticks
        {
            Name = "Ticks",
            Angle = 30,
            OffsetAngle = 30,
            FillBrush = Brushes.Black,
            DistanceFromEdge = 18,
            SkipIndex = 3
        };

        yield return new Blade2Hand
        {
            Name = "Hour Hand",
            TimeComponent = TimeComponent.Hour,
            FillBrush = Brushes.Black,
            Length = 60,
            HipDistance = 29,
            Width = 9,
            StrokeBrush = new SolidColorBrush(Color.FromRgb(0x90, 0x90, 0x90)),
            StrokeThickness = 0.1
        };

        yield return new Blade2Hand
        {
            Name = "Minute Hand",
            TimeComponent = TimeComponent.Minute,
            FillBrush = Brushes.Black,
            Length = 80,
            HipDistance = 43,
            TipLength = 18,
            Width = 9,
            StrokeBrush = new SolidColorBrush(Color.FromRgb(0x90, 0x90, 0x90)),
            StrokeThickness = 0.1
        };

        yield return new SimpleLineHand
        {
            Name = "Second Hand",
            TimeComponent = TimeComponent.Second,
            Length = 70,
            StrokeThickness = 1.5,
            FillBrush = Brushes.Black,
            IntegralValue = true
        };

        yield return new Pin
        {
            Name = "Pin",
            Diameter = 13,
            FillBrush = Brushes.Black
        };
    }
}
