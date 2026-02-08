using DustInTheWind.ClockAvalonia.Demo.MainArea;
using DustInTheWind.ClockAvalonia.Demo.MiscellaneousArea;
using DustInTheWind.ClockAvalonia.Demo.State;
using DustInTheWind.ClockAvalonia.Demo.TemplatesArea;
using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.ClockAvalonia.Demo;

internal static class Setup
{
    public static void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IClockTemplateFactory, ClockTemplateFactory>();
        serviceCollection.AddSingleton<IClockMovementFactory, ClockMovementFactory>();

        AddTemplates(serviceCollection);
        AddMovements(serviceCollection);

        serviceCollection.AddSingleton<ApplicationState>();

        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient<MainWindowViewModel>();

        serviceCollection.AddTransient<TemplatesViewModel>();
        serviceCollection.AddTransient<MiscellaneousViewModel>();

        serviceCollection.AddTransient<AppInitializationUseCase>();
    }

    private static void AddTemplates(ServiceCollection serviceCollection)
    {
        IEnumerable<Type> templateTypes = EnumerateTemplateTypes();

        foreach (Type templateType in templateTypes)
            serviceCollection.AddTransient(templateType);

        serviceCollection.AddSingleton(serviceProvider =>
        {
            IClockTemplateFactory clockTemplateFactory = serviceProvider.GetService<IClockTemplateFactory>();
            ClockTemplatePool clockTemplatePool = new(clockTemplateFactory);

            IEnumerable<Type> templateTypes = EnumerateTemplateTypes();

            clockTemplatePool.AddRange(templateTypes);
            clockTemplatePool.SetDefault<DefaultTemplate>();

            return clockTemplatePool;
        });
    }

    private static IEnumerable<Type> EnumerateTemplateTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && typeof(ClockTemplate).IsAssignableFrom(x));
    }

    private static void AddMovements(ServiceCollection serviceCollection)
    {
        IEnumerable<Type> movementTypes = EnumerateMovementTypes();

        foreach (Type movementType in movementTypes)
            serviceCollection.AddTransient(movementType);

        serviceCollection.AddSingleton(serviceProvider =>
        {
            IClockMovementFactory clockMovementFactory = serviceProvider.GetService<IClockMovementFactory>();
            ClockMovementPool clockMovementPool = new(clockMovementFactory);

            IEnumerable<Type> movementTypes = EnumerateMovementTypes();

            clockMovementPool.AddRange(movementTypes);
            clockMovementPool.SetDefault<LocalTimeMovement>();

            return clockMovementPool;
        });
    }

    private static IEnumerable<Type> EnumerateMovementTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && typeof(IMovement).IsAssignableFrom(x));
    }
}
