# Clock Templates

## Template content

### Shapes

The clock templates are composed of a list of shapes which are applied one over the other.

The templates are inheriting `ClockTemplate` abstract base class.

### Properties

In addition to the list of shapes, the template may provide a number of properties that makes it easier to update different shape properties. If you are familiar to the Facade Pattern, this one is used here.

## Export/Import

A template may be exported in JSON format:

The exported file contains

- template type (optional)
  - If the template type is missing, a generic template is assumed. The generic template does not have additional properties. It has only the list of shapes.
- the list of shapes
  - for each shape:
    - shape type
    - property values

## Creating Templates

Templates group shapes into reusable clock designs.

```csharp
public class MyClockTemplate : ClockTemplate
{
    protected override IEnumerable<Shape> CreateShapes()
    {
        yield return new FlatBackground
        {
            FillBrush = Brushes.White
        };

        yield return new Ticks
        {
            Angle = 6,
            SkipIndex = 5,
            StrokeThickness = 1
        };

        yield return new CapsuleHand
        {
            ComponentToDisplay = TimeComponent.Hour,
            Length = 50,
            Width = 8,
            FillBrush = Brushes.Black
        };

        yield return new CapsuleHand
        {
            ComponentToDisplay = TimeComponent.Minute,
            Length = 85,
            Width = 6,
            FillBrush = Brushes.DarkGray
        };

        yield return new SimpleHand
        {
            ComponentToDisplay = TimeComponent.Second,
            Length = 96,
            StrokeBrush = Brushes.Red,
            IntegralValue = true
        };
    }
}
```

**Apply template**:

```csharp
clock.ClockTemplate = new MyClockTemplate();
```

