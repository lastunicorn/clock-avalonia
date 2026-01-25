using Avalonia;
using Avalonia.Media;
using System.Globalization;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class TextShape : Shape
{
    public const string DefaultName = "Text";
    public const string DefaultText = "Dust in the Wind";
    public const float DefaultMaxWidth = 100f;
    public static Point DefaultLocation = new(0f, 0f);

    #region Text StyledProperty

    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<TextShape, string>(
        nameof(Text),
        defaultValue: DefaultText);

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    #endregion

    #region FontFamily StyledProperty

    public static readonly StyledProperty<FontFamily> FontFamilyProperty = AvaloniaProperty.Register<TextShape, FontFamily>(
        nameof(FontFamily),
        defaultValue: FontFamily.Default);

    public FontFamily FontFamily
    {
        get => GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    #endregion

    #region FontSize StyledProperty

    public static readonly StyledProperty<double> FontSizeProperty = AvaloniaProperty.Register<TextShape, double>(
        nameof(FontSize),
        defaultValue: 12.0);

    public double FontSize
    {
        get => GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    #endregion

    #region FontWeight StyledProperty

    public static readonly StyledProperty<FontWeight> FontWeightProperty = AvaloniaProperty.Register<TextShape, FontWeight>(
        nameof(FontWeight),
        defaultValue: FontWeight.Normal);

    public FontWeight FontWeight
    {
        get => GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    #endregion

    #region MaxWidth StyledProperty

    public static readonly StyledProperty<float> MaxWidthProperty = AvaloniaProperty.Register<TextShape, float>(
        nameof(MaxWidth),
        defaultValue: DefaultMaxWidth);

    public float MaxWidth
    {
        get => GetValue(MaxWidthProperty);
        set => SetValue(MaxWidthProperty, value);
    }

    #endregion

    #region Location StyledProperty

    public static readonly StyledProperty<Point> LocationProperty = AvaloniaProperty.Register<TextShape, Point>(
        nameof(Location),
        defaultValue: DefaultLocation);

    public Point Location
    {
        get => GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    #endregion

    static TextShape()
    {
        NameProperty.OverrideDefaultValue<TextShape>(DefaultName);
        TextProperty.Changed.AddClassHandler<TextShape>((shape, e) => shape.InvalidateLayout());
        FontFamilyProperty.Changed.AddClassHandler<TextShape>((shape, e) => shape.InvalidateLayout());
        FontSizeProperty.Changed.AddClassHandler<TextShape>((shape, e) => shape.InvalidateLayout());
        FontWeightProperty.Changed.AddClassHandler<TextShape>((shape, e) => shape.InvalidateLayout());
        MaxWidthProperty.Changed.AddClassHandler<TextShape>((shape, e) => shape.InvalidateLayout());
        LocationProperty.Changed.AddClassHandler<TextShape>((shape, e) => shape.InvalidateLayout());
    }

    private FormattedText? formattedText;
    private Point textPosition;

    protected override void CalculateLayout(ClockDrawingContext context)
    {
        base.CalculateLayout(context);

        Typeface typeface = new(FontFamily, FontStyle.Normal, FontWeight);

        formattedText = new FormattedText(
            Text,
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            typeface,
            FontSize,
            FillBrush);

        formattedText.MaxTextWidth = MaxWidth;

        Point location = Location;
        double textX = context.ClockRadius * (location.X / 100.0) - formattedText.Width / 2;
        double textY = context.ClockRadius * (location.Y / 100.0) - formattedText.Height / 2;

        textPosition = new Point(textX, textY);
    }

    protected override bool OnRendering(ClockDrawingContext context)
    {
        if (string.IsNullOrEmpty(Text))
            return false;

        return base.OnRendering(context);
    }

    public override void DoRender(ClockDrawingContext context)
    {
        if (formattedText != null)
            context.DrawingContext.DrawText(formattedText, textPosition);
    }
}
