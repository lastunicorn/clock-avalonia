using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.ClearClock.CustomControls;

public class CornerButton : Button
{
    #region Corner Styled Property

    public static readonly StyledProperty<CornerType> CornerProperty =
        AvaloniaProperty.Register<CornerButton, CornerType>(nameof(Corner), CornerType.TopLeft);

    public CornerType Corner
    {
        get => GetValue(CornerProperty);
        set => SetValue(CornerProperty, value);
    }

    #endregion

    #region Data Styled Property

    public static readonly StyledProperty<Geometry> DataProperty =
        AvaloniaProperty.Register<CornerButton, Geometry>(nameof(Data));

    public Geometry Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    #endregion

    #region ContentHorizontalAlignment Styled Property

    public static readonly StyledProperty<HorizontalAlignment> ContentHorizontalAlignmentProperty =
        AvaloniaProperty.Register<CornerButton, HorizontalAlignment>(
            nameof(ContentHorizontalAlignment),
            HorizontalAlignment.Left);

    public HorizontalAlignment ContentHorizontalAlignment
    {
        get => GetValue(ContentHorizontalAlignmentProperty);
        set => SetValue(ContentHorizontalAlignmentProperty, value);
    }

    #endregion

    #region ContentVerticalAlignment Styled Property

    public static readonly StyledProperty<VerticalAlignment> ContentVerticalAlignmentProperty =
        AvaloniaProperty.Register<CornerButton, VerticalAlignment>(
            nameof(ContentVerticalAlignment),
            VerticalAlignment.Top);

    public VerticalAlignment ContentVerticalAlignment
    {
        get => GetValue(ContentVerticalAlignmentProperty);
        set => SetValue(ContentVerticalAlignmentProperty, value);
    }

    #endregion

    #region HoverBackground Styled Property

    public static readonly StyledProperty<IBrush> HoverBackgroundProperty =
        AvaloniaProperty.Register<CornerButton, IBrush>(nameof(HoverBackground));

    public IBrush HoverBackground
    {
        get => GetValue(HoverBackgroundProperty);
        set => SetValue(HoverBackgroundProperty, value);
    }

    #endregion

    static CornerButton()
    {
        CornerProperty.Changed.AddClassHandler<CornerButton>((button, e) => button.UpdateVisualElements());
    }

    public CornerButton()
    {
        UpdateVisualElements();
    }

    private void UpdateVisualElements()
    {
        switch (Corner)
        {
            case CornerType.TopLeft:
                Data = Geometry.Parse("M 0,1 A 2,2 0 0 1 1,0 L 0,0 Z");
                ContentHorizontalAlignment = HorizontalAlignment.Left;
                ContentVerticalAlignment = VerticalAlignment.Top;
                break;

            case CornerType.TopRight:
                Data = Geometry.Parse("M 0,0 A 2,2 0 0 1 1,1 L 1,0 Z");
                ContentHorizontalAlignment = HorizontalAlignment.Right;
                ContentVerticalAlignment = VerticalAlignment.Top;
                break;

            case CornerType.BottomLeft:
                Data = Geometry.Parse("M 0,0 A 2,2 0 0 0 1,1 L 0,1 Z");
                ContentHorizontalAlignment = HorizontalAlignment.Left;
                ContentVerticalAlignment = VerticalAlignment.Bottom;
                break;

            case CornerType.BottomRight:
                Data = Geometry.Parse("M 0,1 A 2,2 0 0 0 1,0 L 1,1 Z");
                ContentHorizontalAlignment = HorizontalAlignment.Right;
                ContentVerticalAlignment = VerticalAlignment.Bottom;
                break;

            default:
                return;
        }
    }
}
