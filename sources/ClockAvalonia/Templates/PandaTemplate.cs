using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Templates;

public class PandaTemplate : ClockTemplate
{
    protected override IEnumerable<Shape> CreateShapes()
    {
        yield return new FlatBackground
        {
            Name = "Background",
            FillBrush = Brushes.White,
            StrokeBrush = Brushes.Black,
            StrokeThickness = 2
        };

        yield return new DotHand
        {
            Name = "Hour Hand",
            TimeComponent = TimeComponent.Hour,
            Length = 50,
            Radius = 25,
            FillBrush = Brushes.Black,
            StrokeBrush = new SolidColorBrush(Color.FromRgb(0x64, 0x64, 0x64)),
            StrokeThickness = 1
        };

        yield return new DotHand
        {
            Name = "Minute Hand",
            TimeComponent = TimeComponent.Minute,
            Length = 50,
            Radius = 15,
            FillBrush = Brushes.Black,
            StrokeBrush = new SolidColorBrush(Color.FromRgb(0x64, 0x64, 0x64)),
            StrokeThickness = 1
        };
    }
}
