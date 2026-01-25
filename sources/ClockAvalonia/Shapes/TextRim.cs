using Avalonia;
using Avalonia.Media;
using System.Globalization;

namespace DustInTheWind.ClockAvalonia.Shapes;

public class TextRim : RimBase
{
    #region Texts StyledProperty

    public static readonly StyledProperty<string[]> TextsProperty = AvaloniaProperty.Register<TextRim, string[]>(
        nameof(Texts),
        defaultValue: null);

    public string[] Texts
    {
        get => GetValue(TextsProperty);
        set => SetValue(TextsProperty, value);
    }

    #endregion

    #region FontFamily StyledProperty

    public static readonly StyledProperty<FontFamily> FontFamilyProperty = AvaloniaProperty.Register<TextRim, FontFamily>(
        nameof(FontFamily),
        defaultValue: FontFamily.Default);

    public FontFamily FontFamily
    {
        get => GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    #endregion

    #region FontSize StyledProperty

    public static readonly StyledProperty<double> FontSizeProperty = AvaloniaProperty.Register<TextRim, double>(
        nameof(FontSize),
        defaultValue: 12.0);

    public double FontSize
    {
        get => GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    #endregion

    #region FontWeight StyledProperty

    public static readonly StyledProperty<FontWeight> FontWeightProperty = AvaloniaProperty.Register<TextRim, FontWeight>(
        nameof(FontWeight),
        defaultValue: FontWeight.Normal);

    public FontWeight FontWeight
    {
        get => GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    #endregion

    private Typeface typeface = Typeface.Default;

    protected override bool OnRendering(ClockDrawingContext context)
    {
        string[] texts = Texts;

        if (texts == null || texts.Length == 0)
            return false;

        typeface = new Typeface(FontFamily, FontStyle.Normal, FontWeight);

        return base.OnRendering(context);
    }

    protected override void RenderItem(DrawingContext drawingContext, int index)
    {
        string[] texts = Texts;

        if (texts == null)
            return;

        if (index >= texts.Length)
            return;

        string text = texts[index];

        if (string.IsNullOrEmpty(text))
            return;

        FormattedText formattedText = new(
            text,
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            typeface,
            FontSize,
            FillBrush);

        double textX = -formattedText.Width / 2;
        double textY = -formattedText.Height / 2;

        Point textPosition = new(textX, textY);
        drawingContext.DrawText(formattedText, textPosition);
    }
}
