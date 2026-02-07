using DustInTheWind.ClockAvalonia.Movements;

namespace DustInTheWind.ClockAvalonia.Demo.State;

public interface IClockMovementFactory
{
    T Create<T>()
        where T : IMovement;

    IMovement Create(Type type);
}
