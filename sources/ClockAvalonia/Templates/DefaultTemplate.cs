using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Templates;

public class DefaultTemplate : ClockTemplate
{
    protected override IEnumerable<Shape> CreateShapes()
    {
        yield return new FlatBackground
        {
            Name = "Background",
            FillBrush = Brushes.WhiteSmoke
        };

        yield return new Ticks
        {
            Name = "Minute Ticks",
            SkipIndex = 5,
            StrokeBrush = new SolidColorBrush(Color.FromRgb(0xa0, 0xa0, 0xa0))
        };

        yield return new Ticks
        {
            Name = "Hour Ticks",
            Angle = 30,
            OffsetAngle = 30,
            StrokeThickness = 1.5
        };

        yield return new Hours
        {
            Name = "Hour Numerals",
            FillBrush = Brushes.Black,
            DistanceFromEdge = 26
        };

        yield return new CapsuleHand
        {
            Name = "Hour Hand",
            TimeComponent = TimeComponent.Hour,
            Length = 48,
            Width = 8,
            TailLength = 4,
            StrokeThickness = 0,
            FillBrush = Brushes.Black
        };

        yield return new CapsuleHand
        {
            Name = "Minute Hand",
            TimeComponent = TimeComponent.Minute,
            Length = 85,
            Width = 8,
            TailLength = 4,
            StrokeThickness = 0,
            FillBrush = Brushes.Black
        };

        yield return new SimpleHand
        {
            Name = "Second Hand",
            TimeComponent = TimeComponent.Second,
            Length = 96,
            TailLength = 14,
            StrokeBrush = Brushes.Red,
            StrokeThickness = 1,
            IntegralValue = true,
            PinDiameter = 8
        };
    }
}
