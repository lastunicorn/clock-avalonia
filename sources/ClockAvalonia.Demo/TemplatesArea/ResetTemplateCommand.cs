using System.Windows.Input;
using DustInTheWind.ClockAvalonia.Demo.State;

namespace DustInTheWind.ClockAvalonia.Demo.TemplatesArea;

public class ResetTemplateCommand : ICommand
{
    private readonly ClockTemplatePool clockTemplatePool;

    public event EventHandler CanExecuteChanged;

    public ResetTemplateCommand(ClockTemplatePool clockTemplatePool)
    {
        this.clockTemplatePool = clockTemplatePool ?? throw new ArgumentNullException(nameof(clockTemplatePool));

        if (clockTemplatePool.CurrentTemplate != null)
            clockTemplatePool.CurrentTemplate.Modified += HandleCurrentTemplateModified;

        clockTemplatePool.CurrentTemplateChanged += HandleCurrentTemplateChanged;
    }

    private void HandleCurrentTemplateChanged(object sender, CurrentTemplateChangedEventArgs e)
    {
        if (e.OldTemplate != null)
            e.OldTemplate.Modified -= HandleCurrentTemplateModified;

        if (e.NewTemplate != null)
            e.NewTemplate.Modified += HandleCurrentTemplateModified;

        OnCanExecuteChanged();
    }

    private void HandleCurrentTemplateModified(object sender, EventArgs e)
    {
        OnCanExecuteChanged();
    }

    public bool CanExecute(object parameter)
    {
        return clockTemplatePool.CurrentTemplate?.IsNew == false;
    }

    public void Execute(object parameter)
    {
        clockTemplatePool.RecreateCurrentTemplate();
    }

    private void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
