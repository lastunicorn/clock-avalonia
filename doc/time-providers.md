# Time Providers

Implement `ITimeProvider` to create custom time sources.

**Built-in providers**:

- `LocalTimeProvider` - System local time
- `UtcTimeProvider` - UTC time
- `BrokenTimeProvider` - Accelerated time (useful for testing)
- `RandomTimeProvider` - Random time values

**Custom provider**:

```csharp
public class MyTimeProvider : TimeProviderBase
{
    protected override TimeSpan GetTime()
    {
        return DateTime.Now.TimeOfDay;
    }
}
```

**Usage**:

```csharp
ITimeProvider timeProvider = new LocalTimeProvider
{
    Interval = 100 // Refresh every 100ms
};
timeProvider.Start();
clock.TimeProvider = timeProvider;
```

