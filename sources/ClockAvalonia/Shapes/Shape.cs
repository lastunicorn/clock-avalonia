using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

public abstract class Shape : AvaloniaObject
{
    private bool isLayoutValid;

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

    static Shape()
    {
        _ = StrokeBrushProperty.Changed.AddClassHandler<Shape>((shape, e) => shape.HandleStrokeBrushChanged(e));
        _ = StrokeThicknessProperty.Changed.AddClassHandler<Shape>((shape, e) => shape.HandleStrokeThicknessChanged(e));
    }

    private IPen strokePen;
    private bool isStrokePenCreated;

    protected IPen StrokePen
    {
        get
        {
            if (!isStrokePenCreated)
            {
                strokePen = CreateStrokePen();
                isStrokePenCreated = true;
            }

            return strokePen;
        }
    }

    protected virtual IPen CreateStrokePen()
    {
        if (StrokeThickness <= 0 || StrokeBrush == null)
            return null;

        return new Pen(StrokeBrush, StrokeThickness);
    }

    private void HandleStrokeBrushChanged(AvaloniaPropertyChangedEventArgs _)
    {
        strokePen = null;
        isStrokePenCreated = false;
    }

    private void HandleStrokeThicknessChanged(AvaloniaPropertyChangedEventArgs _)
    {
        strokePen = null;
        isStrokePenCreated = false;
        InvalidateLayout();
    }

    public event EventHandler NameChanged;

    protected virtual void OnNameChanged(EventArgs e)
    {
        NameChanged?.Invoke(this, e);
    }

    public void Render(ClockDrawingContext context)
    {
        if (!IsVisible)
            return;

        bool allowToRender = OnRendering(context);
        if (!allowToRender)
            return;

        if (!isLayoutValid)
        {
            CalculateLayout(context);
            isLayoutValid = true;
        }

        DoRender(context);

        OnRendered(context);
    }

    protected virtual bool OnRendering(ClockDrawingContext context)
    {
        if (FillBrush == null && StrokePen == null)
            return false;

        return true;
    }

    protected virtual void CalculateLayout(ClockDrawingContext context)
    {
    }

    public abstract void DoRender(ClockDrawingContext context);

    protected virtual void OnRendered(ClockDrawingContext context)
    {
    }

    protected void InvalidateLayout()
    {
        isLayoutValid = false;
    }

    protected virtual void InvalidateDrawingTools()
    {
        if (isStrokePenCreated)
        {
            strokePen = null;
            isStrokePenCreated = false;
        }
    }
}
