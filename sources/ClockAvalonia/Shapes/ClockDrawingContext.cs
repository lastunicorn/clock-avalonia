using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class ClockDrawingContext
{
    public required DrawingContext DrawingContext { get; init; }

    public double ClockDiameter { get; init; }

    public double ClockRadius => ClockDiameter / 2;

    public TimeSpan Time { get; init; }

    public RotationDirection ClockDirection { get; init; }
}
