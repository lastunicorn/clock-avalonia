using System.Collections.ObjectModel;
using DustInTheWind.ClockAvalonia.Demo.State;
using DustInTheWind.ClockAvalonia.Demo.Utils;
using DustInTheWind.ClockAvalonia.Demo.ViewModels;
using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Demo.TemplatesArea;

public class TemplatesViewModel : ViewModelBase
{
    private readonly ClockTemplatePool clockTemplatePool;
    private readonly ClockMovementPool clockMovementPool;

    public ObservableCollection<TemplateDescriptor> TemplateTypes { get; } = [];

    public TemplateDescriptor SelectedTemplateType
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();

            if (!IsInitializing)
                clockTemplatePool.SetDefault(field.Type);
        }
    }

    public ObservableCollection<Shape> Shapes
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();
        }
    }

    public Shape SelectedShape
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();
        }
    }

    public IMovement Movement
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();
        }
    }

    public SaveTemplateCommand SaveTemplateCommand { get; }

    public ResetTemplateCommand ResetTemplateCommand { get; }

    public TemplatesViewModel(ClockTemplatePool clockTemplatePool, ClockMovementPool clockMovementPool)
    {
        this.clockTemplatePool = clockTemplatePool ?? throw new ArgumentNullException(nameof(clockTemplatePool));
        this.clockMovementPool = clockMovementPool ?? throw new ArgumentNullException(nameof(clockMovementPool));

        SaveTemplateCommand = new SaveTemplateCommand(clockTemplatePool);
        ResetTemplateCommand = new ResetTemplateCommand(clockTemplatePool);

        Initialize();

        clockTemplatePool.CurrentTemplateChanged += HandleCurrentTemplateChanged;
        clockMovementPool.CurrentMovementChanged += HandleCurrentMovementChanged;
    }

    private void Initialize()
    {
        Initialize(() =>
        {
            foreach (Type type in clockTemplatePool.EnumerateKnownTypes())
            {
                TemplateTypes.Add(new TemplateDescriptor
                {
                    Name = type.Name
                        .Replace("ClockTemplate", "")
                        .Replace("Template", ""),
                    Type = type
                });
            }

            if (clockTemplatePool.CurrentTemplate != null)
            {
                Type currentTymplateType = clockTemplatePool.CurrentTemplate.GetType();

                SelectedTemplateType = TemplateTypes
                    .FirstOrDefault(x => x.Type == currentTymplateType);

                Shapes = clockTemplatePool.CurrentTemplate
                    .ToObservableCollection();
            }

            Movement = clockMovementPool.CurrentMovement?.Instance;
        });
    }

    private void HandleCurrentMovementChanged(object sender, EventArgs e)
    {
        Movement = clockMovementPool.CurrentMovement?.Instance;
    }

    private void HandleCurrentTemplateChanged(object sender, EventArgs e)
    {
        Shapes = clockTemplatePool.CurrentTemplate
            .ToObservableCollection();
    }
}