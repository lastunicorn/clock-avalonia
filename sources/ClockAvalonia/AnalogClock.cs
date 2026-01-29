using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using DustInTheWind.ClockAvalonia.Shapes;
using DustInTheWind.ClockAvalonia.Templates;
using DustInTheWind.ClockAvalonia.TimeProviders;

namespace DustInTheWind.ClockAvalonia;

public class AnalogClock : TemplatedControl
{
    private Dial dial;

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

    #region TimeProvider StyledProperty

    public static readonly StyledProperty<ITimeProvider?> TimeProviderProperty = AvaloniaProperty.Register<AnalogClock, ITimeProvider>(
        nameof(TimeProvider));

    public ITimeProvider TimeProvider
    {
        get => GetValue(TimeProviderProperty);
        set => SetValue(TimeProviderProperty, value);
    }

    #endregion

    #region ClockTemplate StyledProperty

    public static readonly StyledProperty<ClockTemplate?> ClockTemplateProperty = AvaloniaProperty.Register<AnalogClock, ClockTemplate>(
        nameof(ClockTemplate));

    public ClockTemplate ClockTemplate
    {
        get => GetValue(ClockTemplateProperty);
        set => SetValue(ClockTemplateProperty, value);
    }

    #endregion

    static AnalogClock()
    {
        ShapesProperty.Changed.AddClassHandler<AnalogClock>((clock, e) => clock.OnShapesChanged(e));
        KeepProportionsProperty.Changed.AddClassHandler<AnalogClock>((clock, e) => clock.OnKeepProportionsChanged(e));
        TimeProviderProperty.Changed.AddClassHandler<AnalogClock>((clock, e) => clock.OnTimeProviderChanged(e));
        ClockTemplateProperty.Changed.AddClassHandler<AnalogClock>((clock, e) => clock.OnClockTemplateChanged(e));
    }

    public AnalogClock()
    {
        Shapes = [];
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        dial = e.NameScope.Find<Dial>("PART_Dial");

        ITimeProvider currentTimeProvider = TimeProvider;
        if (currentTimeProvider != null)
            UpdateDisplayedTime(currentTimeProvider.LastValue);
    }

    private void OnShapesChanged(AvaloniaPropertyChangedEventArgs e)
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

    private void OnKeepProportionsChanged(AvaloniaPropertyChangedEventArgs _)
    {
        dial?.InvalidateVisual();
    }

    private void OnTimeProviderChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ITimeProvider oldTimeProvider)
            oldTimeProvider.Tick -= HandleTimeChanged;

        if (e.NewValue is ITimeProvider newTimeProvider)
        {
            newTimeProvider.Tick += HandleTimeChanged;
            UpdateDisplayedTime(newTimeProvider.LastValue);
        }
    }

    private void OnClockTemplateChanged(AvaloniaPropertyChangedEventArgs e)
    {
        Shapes.Clear();

        if (e.NewValue is ClockTemplate clockTemplate)
        {
            foreach (Shape shape in clockTemplate)
                Shapes.Add(shape);
        }
    }

    private void HandleTimeChanged(object sender, TickEventArgs e)
    {
        UpdateDisplayedTime(e.Time);
    }

    private void UpdateDisplayedTime(TimeSpan time)
    {
        if (dial != null)
        {
            Dispatcher.UIThread.Post(() =>
            {
                dial.Time = time;
            });
        }
    }
}
