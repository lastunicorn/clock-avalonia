using DustInTheWind.ClockAvalonia.Demo.State;
using DustInTheWind.ClockAvalonia.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.ClockAvalonia.Demo;

public class ClockTemplateFactory : IClockTemplateFactory
{
    private readonly IServiceProvider serviceProvider;

    public ClockTemplateFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public T Create<T>()
        where T : ClockTemplate
    {
        return serviceProvider.GetService<T>();
    }

    public ClockTemplate Create(Type type)
    {
        return (ClockTemplate)serviceProvider.GetService(type);
    }
}
