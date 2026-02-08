using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Templates;

public class BobTemplate : ClockTemplate
{
    protected override IEnumerable<Shape> CreateShapes()
    {
        FlatBackground background = new()
        {
            Name = "Background",
            FillBrush = Brushes.WhiteSmoke,
            StrokeThickness = 22
        };
        yield return background;

        SimpleLineHand hourHand = new()
        {
            Name = "Hour Hand",
            TimeComponent = TimeComponent.Hour,
            Length = 44,
            TailLength = 9,
            StrokeThickness = 18,
            FillBrush = Brushes.Black,
            RoundEnds = true
        };
        yield return hourHand;

        SimpleLineHand minuteHand = new()
        {
            Name = "Minute Hand",
            TimeComponent = TimeComponent.Minute,
            Length = 69,
            TailLength = 9,
            StrokeThickness = 18,
            FillBrush = Brushes.Black,
            RoundEnds = true
        };
        yield return minuteHand;
    }
}