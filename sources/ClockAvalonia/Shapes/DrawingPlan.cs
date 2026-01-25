using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.Shapes;

internal class DrawingPlan
{
    private readonly DrawingContext drawingContext;
    private readonly List<ITransform> transforms = [];

    private DrawingPlan(DrawingContext drawingContext)
    {
        this.drawingContext = drawingContext ?? throw new ArgumentNullException(nameof(drawingContext));
    }

    public static DrawingPlan Create(DrawingContext drawingContext)
    {
        return new DrawingPlan(drawingContext);
    }

    public DrawingPlan WithTransform<T>(Func<T> createTransform)
        where T : ITransform
    {
        ArgumentNullException.ThrowIfNull(createTransform);

        T transform = createTransform.Invoke();

        if (transform != null)
            transforms.Add(transform);

        return this;
    }

    public void Draw(Action<DrawingContext> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        List<IDisposable> pushedStates = [];

        try
        {
            foreach (ITransform transform in transforms)
            {
                IDisposable state = drawingContext.PushTransform(transform.Value);
                pushedStates.Add(state);
            }

            action?.Invoke(drawingContext);
        }
        finally
        {
            for (int i = pushedStates.Count - 1; i >= 0; i--)
                pushedStates[i].Dispose();
        }
    }
}
