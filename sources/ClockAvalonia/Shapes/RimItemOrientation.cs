namespace DustInTheWind.ClockAvalonia.Shapes;

/// <summary>
/// Specifies the orientation of the numbers that marks the hours.
/// </summary>
public enum RimItemOrientation
{
    /// <summary>
    /// The numbers are displayed as normal text, oriented up-down.
    /// </summary>
    Normal,

    /// <summary>
    /// The numbers are displayed oriented to the center of the clock.
    /// </summary>
    FaceIn,

    /// <summary>
    /// The numbers are displayed oriented to the outside of the clock.
    /// </summary>
    FaceOut,

    /// <summary>
    /// The items displayed in the upper part of the dial, from -90 degrees (inclusive) until
    /// 90 degrees (inclusive) are displayed facing center; the items from the lower part of the
    /// dial are displayed facing out.
    /// </summary>
    HalfInHalfOut,

    /// <summary>
    /// The orientation of the item is decided programatically, by the inheritor class.
    /// </summary>
    Custom
}
