namespace DustInTheWind.ClockAvalonia.Movements;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class MovementAttribute : Attribute
{
    public string Name { get; set; }

    public string Description { get; set; }

    public MovementAttribute()
    {
    }

    public MovementAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
