using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class Hours : TextRim
{
    static Hours()
    {
        DistanceFromEdgeProperty.OverrideDefaultValue<Hours>(25.0);
        FontFamilyProperty.OverrideDefaultValue<Hours>(FontFamily.Default);
        FontSizeProperty.OverrideDefaultValue<Hours>(22.0);
        FontWeightProperty.OverrideDefaultValue<Hours>(FontWeight.Normal);
        TextsProperty.OverrideDefaultValue<Hours>(GenerateHourNumbers());
        AngleProperty.OverrideDefaultValue<Hours>(30.0);
        OffsetAngleProperty.OverrideDefaultValue<Hours>(30.0);
        OrientationProperty.OverrideDefaultValue<Hours>(RimItemOrientation.Normal);
    }

    private static string[] GenerateHourNumbers()
    {
        return Enumerable.Range(1, 12)
            .Select(x => x.ToString())
            .ToArray();
    }
}
