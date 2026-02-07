using DustInTheWind.ClockAvalonia.Demo.State;
using DustInTheWind.ClockAvalonia.Shapes;

namespace DustInTheWind.ClockAvalonia.Demo;

internal class AppInitializationUseCase
{
    private readonly ApplicationState applicationState;

    public AppInitializationUseCase(ApplicationState applicationState)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public void Execute()
    {
        applicationState.ClockDirection = RotationDirection.Clockwise;
    }
}