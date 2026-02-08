using DustInTheWind.ClockAvalonia.Demo.MiscellaneousArea;
using DustInTheWind.ClockAvalonia.Demo.State;
using DustInTheWind.ClockAvalonia.Demo.TemplatesArea;
using DustInTheWind.ClockAvalonia.Demo.ViewModels;
using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Shapes;
using DustInTheWind.ClockAvalonia.Templates;

namespace DustInTheWind.ClockAvalonia.Demo.MainArea;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ApplicationState applicationState;
    private readonly ClockTemplatePool clockTemplatePool;
    private readonly ClockMovementPool clockMovementPool;

    public ClockTemplate ClockTemplate
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

    public IMovement ClockMovement
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

    public RotationDirection ClockDirection
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();

            if (!IsInitializing)
                applicationState.ClockDirection = value;
        }
    }
    public TemplatesViewModel TemplatesViewModel { get; }


    public MiscellaneousViewModel MiscellaneousViewModel { get; }

    public MainWindowViewModel(
        ApplicationState applicationState,
        ClockTemplatePool clockTemplatePool,
        ClockMovementPool clockMovementPool,
        TemplatesViewModel templatesViewModel,
        MiscellaneousViewModel miscellaneousViewModel)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.clockTemplatePool = clockTemplatePool ?? throw new ArgumentNullException(nameof(clockTemplatePool));
        this.clockMovementPool = clockMovementPool ?? throw new ArgumentNullException(nameof(clockMovementPool));

        TemplatesViewModel = templatesViewModel ?? throw new ArgumentNullException(nameof(templatesViewModel));
        MiscellaneousViewModel = miscellaneousViewModel ?? throw new ArgumentNullException(nameof(miscellaneousViewModel));

        clockTemplatePool.CurrentTemplateChanged += HandleCurrentTemplateChanged;
        clockMovementPool.CurrentMovementChanged += HandleCurrentMovementChanged;
        applicationState.ClockDirectionChanged += HandleClockDirectionChanged;

        Initialize();
    }

    private void Initialize()
    {
        Initialize(() =>
        {
            ClockTemplate = clockTemplatePool.CurrentTemplate;
            ClockMovement = clockMovementPool.CurrentMovement?.Instance;
            ClockDirection = applicationState.ClockDirection;
        });
    }

    private void HandleCurrentMovementChanged(object sender, EventArgs e)
    {
        ClockMovement = clockMovementPool.CurrentMovement?.Instance;
    }

    private void HandleCurrentTemplateChanged(object sender, EventArgs e)
    {
        ClockTemplate = clockTemplatePool.CurrentTemplate;
    }

    private void HandleClockDirectionChanged(object sender, EventArgs e)
    {
        Initialize(() =>
        {
            ClockDirection = applicationState.ClockDirection;
        });
    }
}
