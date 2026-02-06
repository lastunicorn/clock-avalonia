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

        yield return new SimpleLineHand
        {
            Name = "Hour Hand",
            TimeComponent = TimeComponent.Hour,
            Length = 48,
            TailLength = 4,
            StrokeThickness = 8,
            FillBrush = Brushes.Black,
            RoundEnds = true
        };

        yield return new SimpleLineHand
        {
            Name = "Minute Hand",
            TimeComponent = TimeComponent.Minute,
            Length = 85,
            TailLength = 4,
            StrokeThickness = 8,
            FillBrush = Brushes.Black,
            RoundEnds = true
        };

        yield return new SimpleLineHand
        {
            Name = "Second Hand",
            TimeComponent = TimeComponent.Second,
            Length = 96.5,
            TailLength = 24,
            StrokeBrush = Brushes.Red,
            StrokeThickness = 1,
            IntegralValue = true
        };

        yield return new Pin()
        {
            Name = "Pin",
            Diameter = 8,
            FillBrush = Brushes.Red,
            StrokeThickness = 0
        };
    }
}
