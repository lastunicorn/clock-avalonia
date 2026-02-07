using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Performance;
using DustInTheWind.ClockAvalonia.Shapes;
using DustInTheWind.ClockAvalonia.Templates;

namespace DustInTheWind.ClockAvalonia;

public class AnalogClock : TemplatedControl
{
    private Dial dial;

    #region PerformanceMeter StyledProperty

    public static readonly StyledProperty<PerformanceMeter> PerformanceMeterProperty = AvaloniaProperty.Register<AnalogClock, PerformanceMeter>(
        nameof(PerformanceMeter),
        defaultValue: null);

    public PerformanceMeter PerformanceMeter
    {
        get => GetValue(PerformanceMeterProperty);
        set => SetValue(PerformanceMeterProperty, value);
    }

    #endregion

    #region Shapes StyledProperty

    public static readonly StyledProperty<ObservableCollection<Shape>> ShapesProperty = AvaloniaProperty.Register<AnalogClock, ObservableCollection<Shape>>(
        nameof(Shapes),
        defaultValue: null);

    public ObservableCollection<Shape> Shapes
    {
        get => GetValue(ShapesProperty);
        set => SetValue(ShapesProperty, value);
    }

    #endregion

    #region IsEmpty StyledProperty

    public static readonly StyledProperty<bool> IsEmptyProperty = AvaloniaProperty.Register<AnalogClock, bool>(
        nameof(IsEmpty),
        defaultValue: true);

    public bool IsEmpty
    {
        get => GetValue(IsEmptyProperty);
        private set => SetValue(IsEmptyProperty, value);
    }

    #endregion

    #region KeepProportions StyledProperty

    public static readonly StyledProperty<bool> KeepProportionsProperty = AvaloniaProperty.Register<AnalogClock, bool>(
        nameof(KeepProportions),
        defaultValue: true);

    public bool KeepProportions
    {
        get => GetValue(KeepProportionsProperty);
        set => SetValue(KeepProportionsProperty, value);
    }

    #endregion

    #region Movement StyledProperty

    public static readonly StyledProperty<IMovement> MovementProperty = AvaloniaProperty.Register<AnalogClock, IMovement>(
        nameof(Movement));

    public IMovement Movement
    {
        get => GetValue(MovementProperty);
        set => SetValue(MovementProperty, value);
    }

    #endregion

    #region ClockTemplate StyledProperty

    public static readonly StyledProperty<ClockTemplate> ClockTemplateProperty = AvaloniaProperty.Register<AnalogClock, ClockTemplate>(
        nameof(ClockTemplate));

    public ClockTemplate ClockTemplate
    {
        get => GetValue(ClockTemplateProperty);
        set => SetValue(ClockTemplateProperty, value);
    }

    #endregion

    #region RotationDirection StyledProperty

    public static readonly StyledProperty<RotationDirection> RotationDirectionProperty = AvaloniaProperty.Register<AnalogClock, RotationDirection>(
        nameof(RotationDirection),
        defaultValue: RotationDirection.Clockwise);

    public RotationDirection RotationDirection
    {
        get => GetValue(RotationDirectionProperty);
        set => SetValue(RotationDirectionProperty, value);
    }

    #endregion

    static AnalogClock()
    {
        ShapesProperty.Changed.AddClassHandler<AnalogClock>((clock, e) => clock.HandleShapesChanged(e));
        KeepProportionsProperty.Changed.AddClassHandler<AnalogClock>((clock, e) => clock.HandleKeepProportionsChanged(e));
        ClockTemplateProperty.Changed.AddClassHandler<AnalogClock>((clock, e) => clock.HandleClockTemplateChanged(e));
    }

    public AnalogClock()
    {
        Shapes = [];
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        dial = e.NameScope.Find<Dial>("PART_Dial");
    }

    private void HandleShapesChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ObservableCollection<Shape> oldShapes)
            oldShapes.CollectionChanged -= HandleShapesCollectionChanged;

        if (e.NewValue is ObservableCollection<Shape> newShapes)
        {
            newShapes.CollectionChanged += HandleShapesCollectionChanged;
            UpdateIsEmpty();
        }
    }

    private void HandleShapesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateIsEmpty();
    }

    private void UpdateIsEmpty()
    {
        IsEmpty = Shapes == null || Shapes.Count == 0;
    }

    private void HandleKeepProportionsChanged(AvaloniaPropertyChangedEventArgs _)
    {
        dial?.InvalidateVisual();
    }

    private void HandleClockTemplateChanged(AvaloniaPropertyChangedEventArgs e)
    {
        Shapes.Clear();

        if (e.NewValue is ClockTemplate clockTemplate)
        {
            foreach (Shape shape in clockTemplate)
                Shapes.Add(shape);
        }
    }
}
