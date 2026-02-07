using DustInTheWind.ClockAvalonia.Movements;

namespace DustInTheWind.ClockAvalonia.Demo.State;

public class MovementDescriptor
{
    public string Name { get; }

    public string Description { get; }

    public Type Type { get; }

    public bool IsNew { get; private set; }

    public IMovement Instance
    {
        get => field;
        private set
        {
            if (field != null)
                field.Modified -= HandleMovementModified;

            field = value;

            IsNew = field != null;

            if (field != null)
                field.Modified += HandleMovementModified;
        }
    }

    private void HandleMovementModified(object sender, EventArgs e)
    {
        IsNew = false;
    }

    public MovementDescriptor(Type type)
    {
        if (!typeof(IMovement).IsAssignableFrom(type))
            throw new ArgumentException($"Type {type.FullName} does not implement IMovement.", nameof(type));

        MovementAttribute movementAttribute = (MovementAttribute)Attribute.GetCustomAttribute(type, typeof(MovementAttribute));

        string name = movementAttribute?.Name ?? type.Name.Replace("Movement", "");
        string description = movementAttribute?.Description ?? string.Empty;

        Name = name;
        Description = description;
        Type = type;
    }

    public void CreateInstance(IClockMovementFactory clockMovementFactory)
    {
        ArgumentNullException.ThrowIfNull(clockMovementFactory);

        IMovement instance = clockMovementFactory.Create(Type);

        if (instance == null)
            throw new Exception("Clock movement could not be created by the factory. Verify that the type was registerd into the dependency container.");

        Instance = instance;
    }
}