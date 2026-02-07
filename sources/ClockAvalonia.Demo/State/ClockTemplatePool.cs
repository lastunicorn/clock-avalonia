using DustInTheWind.ClockAvalonia.Templates;

namespace DustInTheWind.ClockAvalonia.Demo.State;

public class ClockTemplatePool
{
    private readonly IClockTemplateFactory clockTemplateFactory;
    private readonly List<Type> types = [];
    private readonly List<ClockTemplate> instances = [];

    #region CurrentTemplate Property

    public ClockTemplate CurrentTemplate
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            ClockTemplate oldTemplate = value;
            field = value;
            ClockTemplate newTemplate = value;

            CurrentTemplateChangedEventArgs args = new(oldTemplate, newTemplate);
            OnCurrentTemplateChanged(args);
        }
    }

    public event EventHandler<CurrentTemplateChangedEventArgs> CurrentTemplateChanged;

    private void OnCurrentTemplateChanged(CurrentTemplateChangedEventArgs e)
    {
        CurrentTemplateChanged?.Invoke(this, e);
    }

    #endregion

    public ClockTemplatePool(IClockTemplateFactory clockTemplateFactory)
    {
        this.clockTemplateFactory = clockTemplateFactory ?? throw new ArgumentNullException(nameof(clockTemplateFactory));
    }

    public void AddRange(IEnumerable<Type> types)
    {
        ArgumentNullException.ThrowIfNull(types);

        foreach (Type type in types)
        {
            if (typeof(ClockTemplate).IsAssignableFrom(type))
                this.types.Add(type);
        }
    }

    public void SetDefault(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        bool isClockTemplate = typeof(ClockTemplate).IsAssignableFrom(type);
        if (!isClockTemplate)
            throw new ArgumentException($"The type {type.FullName} is not a clock template.");

        ClockTemplate instance = instances
            .FirstOrDefault(x => x.GetType() == type);

        instance ??= Create(type);

        CurrentTemplate = instance;
    }

    public void RecreateCurrentTemplate()
    {
        ClockTemplate oldInstance = CurrentTemplate;

        if (oldInstance == null)
            return;

        instances.Remove(oldInstance);

        Type type = oldInstance.GetType();

        ClockTemplate newInstance = Create(type);
        CurrentTemplate = newInstance;
    }

    private ClockTemplate Create(Type type)
    {
        ClockTemplate instance = clockTemplateFactory.Create(type);

        if (instance == null)
            throw new Exception("Clock template could not be created by the factory. Verify that the type was registerd into the dependency container.");

        instances.Add(instance);

        bool typeExists = types.Any(x => x == type);

        if (!typeExists)
            types.Add(type);

        return instance;
    }

    public void SetDefault<T>()
        where T : ClockTemplate
    {
        T instance = instances
            .OfType<T>()
            .FirstOrDefault();

        instance ??= Create<T>();

        CurrentTemplate = instance;
    }

    private T Create<T>()
        where T : ClockTemplate
    {
        T instance = clockTemplateFactory.Create<T>();

        if (instance == null)
            throw new Exception("Clock template could not be created by the factory. Verify that the type was registerd into the dependency container.");

        instances.Add(instance);

        bool typeExists = types.Any(x => x == typeof(T));

        if (!typeExists)
            types.Add(typeof(T));

        return instance;
    }

    public IEnumerable<Type> EnumerateKnownTypes()
    {
        return types;
    }
}