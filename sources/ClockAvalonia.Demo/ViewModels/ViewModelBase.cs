using CommunityToolkit.Mvvm.ComponentModel;

namespace DustInTheWind.ClockAvalonia.Demo.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected bool IsInitializing { get; private set; }

    protected Task Initialize(Func<Task> action)
    {
        IsInitializing = true;
        try
        {
            return action?.Invoke();
        }
        finally
        {
            IsInitializing = false;
        }
    }

    protected void Initialize(Action action)
    {
        IsInitializing = true;
        try
        {
            action?.Invoke();
        }
        finally
        {
            IsInitializing = false;
        }
    }
}
