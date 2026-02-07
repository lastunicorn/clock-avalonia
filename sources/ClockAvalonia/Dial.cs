using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Performance;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia;

public class Dial : Control
{
    private NotifyCollectionChangedEventHandler collectionChangedHandler;
    private TimeSpan time;

    #region PerformanceMeter StyledProperty

    public static readonly StyledProperty<PerformanceMeter> PerformanceMeterProperty = AvaloniaProperty.Register<Dial, PerformanceMeter>(
        nameof(PerformanceMeter),
        defaultValue: null);

    public PerformanceMeter PerformanceMeter
    {
        get => GetValue(PerformanceMeterProperty);
        set => SetValue(PerformanceMeterProperty, value);
    }

    #endregion

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

    #region Movement StyledProperty

    public static readonly StyledProperty<IMovement> MovementProperty = AvaloniaProperty.Register<Dial, IMovement>(
        nameof(Movement),
        defaultValue: null);

    public IMovement Movement
    {
        get => GetValue(MovementProperty);
        set => SetValue(MovementProperty, value);
    }

    #endregion

    #region RotationDirection StyledProperty

    public static readonly StyledProperty<RotationDirection> RotationDirectionProperty = AvaloniaProperty.Register<Dial, RotationDirection>(
        nameof(RotationDirection),
        defaultValue: RotationDirection.Clockwise);

    public RotationDirection RotationDirection
    {
        get => GetValue(RotationDirectionProperty);
        set => SetValue(RotationDirectionProperty, value);
    }

    #endregion

    static Dial()
    {
        _ = ShapesProperty.Changed.AddClassHandler<Dial>((canvas, e) => canvas.HandleShapesChanged(e));
        _ = KeepProportionsProperty.Changed.AddClassHandler<Dial>((canvas, e) => canvas.HandleKeepProportionsChanged(e));
        _ = MovementProperty.Changed.AddClassHandler<Dial>((canvas, e) => canvas.HandleMovementChanged(e));
        _ = RotationDirectionProperty.Changed.AddClassHandler<Dial>((canvas, e) => canvas.HandleRotationDirectionChnaged(e));

        AffectsRender<Dial>(ShapesProperty, KeepProportionsProperty, MovementProperty);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        double width = double.IsInfinity(availableSize.Width) ? 200 : availableSize.Width;
        double height = double.IsInfinity(availableSize.Height) ? 200 : availableSize.Height;
        double size = Math.Min(width, height);

        return new Size(size, size);
    }

    private void HandleShapesChanged(AvaloniaPropertyChangedEventArgs e)
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

    private void HandleKeepProportionsChanged(AvaloniaPropertyChangedEventArgs e)
    {
        InvalidateVisual();
    }

    private void HandleMovementChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is IMovement oldMovement)
            oldMovement.Tick -= HandleTick;

        if (e.NewValue is IMovement newMovement)
        {
            newMovement.Tick += HandleTick;

            time = newMovement.LastTick;
            InvalidateVisual();
        }
    }

    private void HandleTick(object sender, TickEventArgs e)
    {
        time = e.Time;

        try
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                InvalidateVisual();
            });
        }
        catch (TaskCanceledException)
        {
            // Ignore
        }
    }

    private void HandleRotationDirectionChnaged(AvaloniaPropertyChangedEventArgs e)
    {
        InvalidateVisual();
    }

    public override void Render(DrawingContext drawingContext)
    {
        PerformanceMeter performanceInfo = PerformanceMeter;
        performanceInfo?.StartMeasurement();

        try
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
        finally
        {
            performanceInfo?.EndMeasurement();
        }
    }

    private void RenderShapes(DrawingContext drawingContext, double diameter)
    {
        ClockDrawingContext clockDrawingContext = new()
        {
            DrawingContext = drawingContext,
            ClockDiameter = diameter,
            Time = time,
            ClockDirection = RotationDirection
        };

        foreach (Shape shape in Shapes)
            shape?.Render(clockDrawingContext);
    }
}
