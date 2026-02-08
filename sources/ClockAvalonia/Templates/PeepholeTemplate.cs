

using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Templates;

public class PeepholeTemplate : ClockTemplate
{
    protected override IEnumerable<Shape> CreateShapes()
    {
        yield return new FlatBackground
        {
            Name = "Background",
            FillBrush = Brushes.Black
        };

        yield return new HourNumerals
        {
            Name = "Hour Numerals",
            FillBrush = Brushes.White,
            FontFamily = new FontFamily("Arial"),
            FontSize = 26,
            FontWeight = FontWeight.Bold,
            DistanceFromEdge = 46
        };

        yield return new SlotHand
        {
            Name = "Hour Hand",
            TimeComponent = TimeComponent.Hour,
            FillBrush = Brushes.White,
            Radius = 98,
            Length = 72,
            Width = 23,
            TailLength = 11.5
        };

        yield return new SimpleLineHand
        {
            Name = "Minute Hand",
            TimeComponent = TimeComponent.Minute,
            StrokeBrush = Brushes.Black,
            Length = 90,
            StrokeThickness = 8,
            TailLength = 0
        };

        yield return new SimpleLineHand
        {
            Name = "Second Hand",
            TimeComponent = TimeComponent.Second,
            StrokeBrush = Brushes.Black,
            Length = 90,
            StrokeThickness = 1,
            TailLength = 30
        };

        yield return new Pin
        {
            Name = "Pin",
            FillBrush = Brushes.Black,
            Diameter = 20,
            StrokeBrush = new SolidColorBrush(Color.FromRgb(0x64, 0x64, 0x64)),
            StrokeThickness = 0.5
        };
    }
}
