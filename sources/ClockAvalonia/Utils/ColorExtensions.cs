using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Utils;

namespace DustInTheWind.ClockWpf.Utils;

public static class ColorExtensions
{
    public static Color ShiftHue(this Color color, float hueDelta)
    {
        return HsbColor.FromColor(color)
            .ShiftHue(hueDelta)
            .ToColor();
    }

    public static Color ShiftSaturation(this Color color, float saturationDelta)
    {
        return HsbColor.FromColor(color)
            .ShiftSaturation(saturationDelta)
            .ToColor();
    }

    public static Color ShiftBrighness(this Color color, float brightnessDelta)
    {
        return HsbColor.FromColor(color)
            .ShiftBrighness(brightnessDelta)
            .ToColor();
    }
}
