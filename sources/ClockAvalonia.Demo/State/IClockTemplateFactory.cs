using DustInTheWind.ClockAvalonia.Templates;

namespace DustInTheWind.ClockAvalonia.Demo.State;

public interface IClockTemplateFactory
{
    T Create<T>()
        where T : ClockTemplate;

    ClockTemplate Create(Type type);
}
