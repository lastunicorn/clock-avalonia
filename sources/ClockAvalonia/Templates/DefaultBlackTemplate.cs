using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Templates;

public class DefaultBlackTemplate : ClockTemplate
{
    protected override IEnumerable<Shape> CreateShapes()
    {
        FlatBackground background = new()
        {
            Name = "Background",
            FillBrush = Brushes.Black
        };
        yield return background;

        Ticks minuteTicks = new()
        {
            Name = "Minute Ticks",
            SkipIndex = 5,
            StrokeBrush = new SolidColorBrush(Color.FromRgb(0x60, 0x60, 0x60))
        };
        yield return minuteTicks;

        Ticks hourTicks = new()
        {
            Name = "Hour Ticks",
            Angle = 30,
            OffsetAngle = 30,
            StrokeThickness = 1.5,
            StrokeBrush = Brushes.WhiteSmoke
        };
        yield return hourTicks;

        HourNumerals hourNumerals = new()
        {
            Name = "Hour Numerals",
            FillBrush = Brushes.WhiteSmoke,
            DistanceFromEdge = 26
        };
        yield return hourNumerals;

        RodHand hourHand = new()
        {
            Name = "Hour Hand",
            TimeComponent = TimeComponent.Hour,
            Length = 48,
            Width = 8,
            TailLength = 4,
            StrokeThickness = 0,
            FillBrush = Brushes.WhiteSmoke
        };
        yield return hourHand;

        RodHand minuteHand = new()
        {
            Name = "Minute Hand",
            TimeComponent = TimeComponent.Minute,
            Length = 85,
            Width = 8,
            TailLength = 4,
            StrokeThickness = 0,
            FillBrush = Brushes.WhiteSmoke
        };
        yield return minuteHand;

        SimpleLineHand secondHand = new()
        {
            Name = "Second Hand",
            TimeComponent = TimeComponent.Second,
            Length = 96.5,
            TailLength = 24,
            StrokeBrush = Brushes.OrangeRed,
            StrokeThickness = 1,
            IntegralValue = true
        };
        yield return secondHand;

        Pin pin = new()
        {
            Name = "Pin",
            Diameter = 8,
            FillBrush = Brushes.OrangeRed
        };
        yield return pin;
    }
}