using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class HourNumerals : TextRim
{
    static HourNumerals()
    {
        DistanceFromEdgeProperty.OverrideDefaultValue<HourNumerals>(25.0);
        FontSizeProperty.OverrideDefaultValue<HourNumerals>(22.0);
        FontWeightProperty.OverrideDefaultValue<HourNumerals>(FontWeight.Normal);
        TextsProperty.OverrideDefaultValue<HourNumerals>(GenerateHourNumbers());
        AngleProperty.OverrideDefaultValue<HourNumerals>(30.0);
        OffsetAngleProperty.OverrideDefaultValue<HourNumerals>(30.0);
        OrientationProperty.OverrideDefaultValue<HourNumerals>(RimItemOrientation.Normal);
    }

    private static string[] GenerateHourNumbers()
    {
        return Enumerable.Range(1, 12)
            .Select(x => x.ToString())
            .ToArray();
    }
}
