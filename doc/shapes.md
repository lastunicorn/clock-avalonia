# Clock Shapes

## Shape Types

### 1. Simple Shapes

Inherit directly from `Shape`. Origin (0,0) is the clock center.

**Built-in types**: `FlatBackground`, `FancyBackground`, `Pin`

**Example**:

```csharp
public class Pin : Shape
{
    public double Diameter { get; set; }

    public override void DoRender(ClockDrawingContext context)
    {
        double radius = context.ClockRadius * (Diameter / 100.0) / 2;
        Point center = new(0, 0);
        context.DrawingContext.DrawEllipse(FillBrush, StrokePen, center, radius, radius);
    }
}
```

### 2. Rim Shapes

Inherit from `RimBase`. Items are auto-positioned around the dial edge.

**Built-in types**: `Ticks`, `Hours`

**Key properties**:

- `Angle` - Degrees between items (default: 30°)
- `OffsetAngle` - Starting rotation offset (default: 0°)
- `DistanceFromEdge` - Distance from dial edge as % of radius
- `Orientation` - `FaceCenter`, `FaceOut`, or `Normal`
- `SkipIndex` - Skip every nth item (useful for alternating patterns)

**Example**:

```csharp
public class Ticks : RimBase
{
    public double Length { get; set; }

    protected override void RenderItem(DrawingContext drawingContext, int index)
    {
        double actualLength = context.ClockRadius * Length / 100.0;
        Point startPoint = new(0, -actualLength / 2);
        Point endPoint = new(0, actualLength / 2);
        drawingContext.DrawLine(StrokePen, startPoint, endPoint);
    }
}
```

**Usage**:

```csharp
new Ticks
{
    Angle = 6,           // Every 6° (60 ticks total)
    SkipIndex = 5,       // Skip every 5th tick
    Length = 5,          // 5% of radius
    DistanceFromEdge = 6 // 6% from edge
};
```

### 3. Hand Shapes

Inherit from `HandBase`. Auto-rotate based on time.

**Built-in types**: `SimpleHand`, `CapsuleHand`, `NibHand`, `DiamondHand`, `DotHand`

**Key properties**:

- `ComponentToDisplay` - `Hour`, `Minute`, or `Second`
- `Length` - Hand length as % of radius
- `IntegralValue` - If `true`, hand jumps discretely; if `false`, moves smoothly

**Example**:

```csharp
public class SimpleHand : HandBase
{
    public double TailLength { get; set; }
    public double PinDiameter { get; set; }

    public override void DoRender(ClockDrawingContext context)
    {
        context.DrawingContext.CreateDrawingPlan()
            .WithTransform(() =>
            {
                double angleDegrees = CalculateHandAngle(context.Time);
                return new RotateTransform(angleDegrees, 0, 0);
            })
            .Draw(dc => DrawHand(dc, context.ClockRadius));
    }
}
```

**Usage**:

```csharp
new SimpleHand
{
    ComponentToDisplay = TimeComponent.Second,
    Length = 96,
    TailLength = 14,
    IntegralValue = true,
    StrokeThickness = 1,
    StrokeBrush = Brushes.Red
};
```

## Common Shape Properties

All shapes inherit these properties:

- `Name` - User-friendly identifier
- `IsVisible` - Show/hide the shape
- `FillBrush` - Fill color/brush
- `StrokeBrush` - Outline color/brush
- `StrokeThickness` - Outline width

## Advanced Shape Development

### Using `ClockDrawingContext`

```csharp
public override void DoRender(ClockDrawingContext context)
{
    double radius = context.ClockRadius;        // Half of ClockDiameter
    double diameter = context.ClockDiameter;     // Full clock size
    TimeSpan time = context.Time;                // Current time to display
    DrawingContext dc = context.DrawingContext;  // Avalonia drawing context
}
```

### Layout Optimization

Override `CalculateLayout()` for expensive calculations that don't change on every render:

```csharp
private Point calculatedPosition;
private double calculatedSize;

protected override void CalculateLayout(ClockDrawingContext context)
{
    base.CalculateLayout(context);
    calculatedPosition = new Point(context.ClockRadius * 0.5, 0);
    calculatedSize = context.ClockRadius * 0.1;
}

public override void DoRender(ClockDrawingContext context)
{
    context.DrawingContext.DrawEllipse(
        FillBrush, null, calculatedPosition, calculatedSize, calculatedSize);
}
```

Invalidate layout when properties change:

```csharp
private static void HandlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
{
    if (d is MyShape shape)
        shape.InvalidateLayout();
}
```

### Transformations

Use `DrawingPlan` for complex transformations:

```csharp
context.DrawingContext.CreateDrawingPlan()
    .WithTransform(() => new RotateTransform(45, 0, 0))
    .WithTransform(() => new TranslateTransform(10, 20))
    .Draw(dc => dc.DrawEllipse(FillBrush, null, new Point(0, 0), 5, 5));
```

