using DustInTheWind.ClockAvalonia.Movements;

namespace DustInTheWind.ClockAvalonia.Demo.State;

public class CurrentMovementChangedEventArgs : EventArgs
{
    public IMovement OldMovement { get; }

    public IMovement NewMovement { get; }

    public CurrentMovementChangedEventArgs(IMovement oldMovement, IMovement newMovement)
    {
        OldMovement = oldMovement;
        NewMovement = newMovement;
    }
}
