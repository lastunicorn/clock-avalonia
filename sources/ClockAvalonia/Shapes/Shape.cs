using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public abstract class Shape : AvaloniaObject
{
    private bool isCacheValid;

    #region Name StyledProperty

    public static readonly StyledProperty<string> NameProperty = AvaloniaProperty.Register<Shape, string>(
        nameof(Name),
        defaultValue: "Shape");

    public string Name
    {
        get => GetValue(NameProperty);
        set => SetValue(NameProperty, value ?? throw new ArgumentNullException(nameof(value)));
    }

    #endregion

    #region IsVisible StyledProperty

    public static readonly StyledProperty<bool> IsVisibleProperty = AvaloniaProperty.Register<Shape, bool>(
        nameof(IsVisible),
        defaultValue: true);

    public bool IsVisible
    {
        get => GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    #endregion

    #region FillBrush StyledProperty

    public static readonly StyledProperty<IBrush> FillBrushProperty = AvaloniaProperty.Register<Shape, IBrush>(
        nameof(FillBrush),
        defaultValue: Brushes.CornflowerBlue);

    public IBrush FillBrush
    {
        get => GetValue(FillBrushProperty);
        set => SetValue(FillBrushProperty, value);
    }

    #endregion

    #region StrokeBrush StyledProperty

    public static readonly StyledProperty<IBrush> StrokeBrushProperty = AvaloniaProperty.Register<Shape, IBrush>(
        nameof(StrokeBrush),
        defaultValue: Brushes.Black);

    public IBrush StrokeBrush
    {
        get => GetValue(StrokeBrushProperty);
        set => SetValue(StrokeBrushProperty, value);
    }

    #endregion

    #region StrokeThickness StyledProperty

    public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<Shape, double>(
        nameof(StrokeThickness),
        defaultValue: 1.0);

    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    #endregion

    #region NameChanged Event

    /// <summary>
    /// Event raised when the <see cref="Name"/> property is changed.
    /// </summary>
    public event EventHandler NameChanged;

    /// <summary>
    /// Raises the <see cref="NameChanged"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
    protected virtual void OnNameChanged(EventArgs e)
    {
        NameChanged?.Invoke(this, e);
    }

    #endregion

    #region Changed Event

    /// <summary>
    /// Event raised when any property that impacts the way the shape looks changed.
    /// </summary>
    public event EventHandler Changed;

    /// <summary>
    /// Raises the <see cref="Changed"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
    protected virtual void OnChanged(EventArgs e)
    {
        Changed?.Invoke(this, e);
    }

    #endregion

    static Shape()
    {
        _ = NameProperty.Changed.AddClassHandler<Shape>((shape, e) => shape.HandleNameChanged(e));
        _ = StrokeBrushProperty.Changed.AddClassHandler<Shape>((shape, e) => shape.HandleStrokeBrushChanged(e));
        _ = StrokeThicknessProperty.Changed.AddClassHandler<Shape>((shape, e) => shape.HandleStrokeThicknessChanged(e));
    }

    private void HandleNameChanged(AvaloniaPropertyChangedEventArgs e)
    {
        OnNameChanged(EventArgs.Empty);
    }

    protected virtual void OnCreateStrokePen(CreateStrokePenEventArgs e)
    {
    }

    private void HandleStrokeBrushChanged(AvaloniaPropertyChangedEventArgs _)
    {
        InvalidateCache();
        OnChanged(EventArgs.Empty);
    }

    private void HandleStrokeThicknessChanged(AvaloniaPropertyChangedEventArgs _)
    {
        InvalidateCache();
        OnChanged(EventArgs.Empty);
    }

    protected Pen CreateStrokePen()
    {
        if (StrokeThickness <= 0 || StrokeBrush == null)
            return null;

        Pen pen = new(StrokeBrush, StrokeThickness);

        CreateStrokePenEventArgs args = new(pen);
        OnCreateStrokePen(args);

        return pen;
    }

    public void Render(ClockDrawingContext context)
    {
        if (!IsVisible)
            return;

        bool allowToRender = OnRendering(context);
        if (!allowToRender)
            return;

        if (!isCacheValid)
        {
            CalculateCache(context);
            isCacheValid = true;
        }

        DoRender(context);

        OnRendered(context);
    }

    protected virtual bool OnRendering(ClockDrawingContext context)
    {
        if (FillBrush == null && (StrokeBrush == null || StrokeThickness <= 0))
            return false;

        return true;
    }

    protected virtual void CalculateCache(ClockDrawingContext context)
    {
    }

    protected abstract void DoRender(ClockDrawingContext context);

    protected virtual void OnRendered(ClockDrawingContext context)
    {
    }

    protected void InvalidateCache()
    {
        isCacheValid = false;
    }
}
