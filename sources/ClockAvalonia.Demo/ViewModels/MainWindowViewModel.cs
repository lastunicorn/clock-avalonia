using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Templates;

namespace DustInTheWind.ClockAvalonia.Demo.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
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

    public MainWindowViewModel()
    {
        ClockTemplate = new DefaultTemplate();
        
        LocalTimeMovement localTimeMovement = new();
        localTimeMovement.Start();

        ClockMovement = localTimeMovement;
    }
}
