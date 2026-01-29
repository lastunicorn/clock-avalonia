using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia;

public class Dial : Control
{
    private NotifyCollectionChangedEventHandler collectionChangedHandler;

    #region Shapes StyledProperty

    public static readonly StyledProperty<ObservableCollection<Shape>> ShapesProperty = AvaloniaProperty.Register<Dial, ObservableCollection<Shape>>(
        nameof(Shapes),
        defaultValue: null);

    public ObservableCollection<Shape> Shapes
    {
        get => GetValue(ShapesProperty);
        set => SetValue(ShapesProperty, value);
    }

    #endregion

    #region KeepProperties StyledProperty

    public static readonly StyledProperty<bool> KeepProportionsProperty = AvaloniaProperty.Register<Dial, bool>(
        nameof(KeepProportions),
        defaultValue: false);

    public bool KeepProportions
    {
        get => GetValue(KeepProportionsProperty);
        set => SetValue(KeepProportionsProperty, value);
    }

    #endregion

    #region Time StyledProperty

    public static readonly StyledProperty<TimeSpan> TimeProperty = AvaloniaProperty.Register<Dial, TimeSpan>(
        nameof(Time),
        defaultValue: TimeSpan.Zero);

    public TimeSpan Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    #endregion

    static Dial()
    {
        ShapesProperty.Changed.AddClassHandler<Dial>((canvas, e) => canvas.OnShapesChanged(e));
        KeepProportionsProperty.Changed.AddClassHandler<Dial>((canvas, e) => canvas.InvalidateVisual());
        TimeProperty.Changed.AddClassHandler<Dial>((canvas, e) => canvas.InvalidateVisual());

        AffectsRender<Dial>(ShapesProperty, KeepProportionsProperty, TimeProperty);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        double width = double.IsInfinity(availableSize.Width) ? 200 : availableSize.Width;
        double height = double.IsInfinity(availableSize.Height) ? 200 : availableSize.Height;
        double size = Math.Min(width, height);

        return new Size(size, size);
    }

    private void OnShapesChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ObservableCollection<Shape> oldShapes && collectionChangedHandler != null)
        {
            oldShapes.CollectionChanged -= collectionChangedHandler;
            collectionChangedHandler = null;
        }

        if (e.NewValue is ObservableCollection<Shape> newShapes)
        {
            collectionChangedHandler = (s, args) => InvalidateVisual();
            newShapes.CollectionChanged += collectionChangedHandler;

            InvalidateVisual();
        }
    }

    public override void Render(DrawingContext drawingContext)
    {
        base.Render(drawingContext);

        if (Shapes == null || Shapes.Count == 0)
            return;

        double diameter = Math.Min(Bounds.Width, Bounds.Height);

        using (drawingContext.PushTransform(Matrix.CreateTranslation(Bounds.Width / 2, Bounds.Height / 2)))
        {
            if (!KeepProportions)
            {
                double scaleX = Bounds.Width / diameter;
                double scaleY = Bounds.Height / diameter;

                using (drawingContext.PushTransform(Matrix.CreateScale(scaleX, scaleY)))
                    RenderShapes(drawingContext, diameter);
            }
            else
            {
                RenderShapes(drawingContext, diameter);
            }
        }
    }

    private void RenderShapes(DrawingContext drawingContext, double diameter)
    {
        ClockDrawingContext clockDrawingContext = new()
        {
            DrawingContext = drawingContext,
            ClockDiameter = diameter,
            Time = Time
        };

        foreach (Shape shape in Shapes)
            shape?.Render(clockDrawingContext);
    }
}
