using DustInTheWind.ClockAvalonia.Movements;

namespace DustInTheWind.ClockAvalonia.Demo.State;

public class ClockMovementPool
{
    private readonly IClockMovementFactory clockMovementFactory;
    private readonly List<MovementDescriptor> descriptors = [];

    #region CurrentMovement Property

    public MovementDescriptor CurrentMovement
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            MovementDescriptor oldMovement = value;
            oldMovement?.Instance?.Stop();

            field = value;

            MovementDescriptor newMovement = value;
            newMovement?.Instance?.Start();

            CurrentMovementChangedEventArgs args = new(oldMovement?.Instance, newMovement?.Instance);
            OnCurrentMovementChanged(args);
        }
    }

    public event EventHandler<CurrentMovementChangedEventArgs> CurrentMovementChanged;

    private void OnCurrentMovementChanged(CurrentMovementChangedEventArgs e)
    {
        CurrentMovementChanged?.Invoke(this, e);
    }

    #endregion

    public ClockMovementPool(IClockMovementFactory clockMovementFactory)
    {
        this.clockMovementFactory = clockMovementFactory ?? throw new ArgumentNullException(nameof(clockMovementFactory));
    }

    public void AddRange(IEnumerable<Type> types)
    {
        ArgumentNullException.ThrowIfNull(types);

        foreach (Type type in types)
            AddDescriptorFor(type);
    }

    private void AddDescriptorFor(Type type)
    {
        MovementDescriptor movementDescriptor = new(type);
        descriptors.Add(movementDescriptor);
    }

    public void SetDefault(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        bool isClockMovement = typeof(IMovement).IsAssignableFrom(type);
        if (!isClockMovement)
            throw new ArgumentException($"The type {type.FullName} is not a clock movement.");

        MovementDescriptor descriptor = descriptors
            .FirstOrDefault(x => x.Type == type);

        if (descriptor == null)
        {
            descriptor = new MovementDescriptor(type);
            descriptors.Add(descriptor);
        }

        if (descriptor.Instance == null)
            descriptor.CreateInstance(clockMovementFactory);

        CurrentMovement = descriptor;
    }

    //private IMovement Create(Type type)
    //{
    //    IMovement instance = clockMovementFactory.Create(type);

    //    if (instance == null)
    //        throw new Exception("Clock movement could not be created by the factory. Verify that the type was registerd into the dependency container.");

    //    instances.Add(instance);

    //    bool typeExists = descriptors.Any(x => x.Type == type);

    //    if (!typeExists)
    //        AddDescriptorFor(type);

    //    return instance;
    //}

    public void SetDefault<T>()
        where T : IMovement
    {
        MovementDescriptor descriptor = descriptors
            .Where(x => x.Type == typeof(T))
            .FirstOrDefault();

        if (descriptor == null)
        {
            descriptor = new MovementDescriptor(typeof(T));
            descriptors.Add(descriptor);
        }

        if (descriptor.Instance == null)
            descriptor.CreateInstance(clockMovementFactory);

        CurrentMovement = descriptor;
    }

    //private T Create<T>()
    //    where T : IMovement
    //{
    //    T instance = clockMovementFactory.Create<T>();

    //    if (instance == null)
    //        throw new Exception("Clock movement could not be created by the factory. Verify that the type was registerd into the dependency container.");

    //    instances.Add(instance);

    //    bool typeExists = descriptors.Any(x => x.Type == typeof(T));

    //    if (!typeExists)
    //        AddDescriptorFor(typeof(T));

    //    return instance;
    //}

    public void RecreateCurrentTemplate()
    {
        MovementDescriptor descriptor = CurrentMovement;

        if (descriptor == null)
            return;

        CurrentMovement = null;
        descriptor.CreateInstance(clockMovementFactory);
        CurrentMovement = descriptor;

        //instances.Remove(oldInstance);

        //Type type = oldInstance.GetType();

        //ClockTemplate newInstance = Create(type);
        //CurrentTemplate = newInstance;
    }

    public IEnumerable<MovementDescriptor> EnumerateKnownMovements()
    {
        return descriptors;
    }
}
