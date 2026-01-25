# Clock Avalonia

## Basic Usage

```xaml
<Window xmlns:clock="using:DustInTheWind.ClockAvalonia">
    <clock:AnalogClock Width="200" Height="200" />
</Window>
```

```csharp
AnalogClock clock = new()
{
    TimeProvider = new LocalTimeProvider(),
    ClockTemplate = new DefaultTemplate()
};
clock.TimeProvider.Start();
```

### Key Properties

- **`TimeProvider`** (`ITimeProvider`) - Controls the time source (local, UTC, broken time, etc.)
- **`ClockTemplate`** (`ClockTemplate`) - Applies a predefined visual template
- **`Shapes`** (`ObservableCollection<Shape>`) - Collection of shapes to render (background, hands, ticks, etc.)

## Architecture

The control renders shapes in layers, stacked from first to last:

1. **Simple Shapes** - Static elements (backgrounds, pins, text) drawn relative to center (0,0)
2. **Rim Shapes** - Repeated items around the dial edge (ticks, hour numerals)
3. **Hand Shapes** - Auto-rotating time indicators (hour, minute, second hands)

