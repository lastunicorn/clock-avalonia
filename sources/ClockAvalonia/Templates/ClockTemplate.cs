using System.Collections.ObjectModel;
using DustInTheWind.ClockAvalonia.Shapes;


namespace DustInTheWind.ClockAvalonia.Templates;

public abstract class ClockTemplate : Collection<Shape>
{
    private bool isNew = true;

    public bool IsNew
    {
        get => isNew;
        private set
        {
            if (isNew == value)
                return;

            isNew = value;
            OnModified();
        }
    }

    private void OnModified()
    {
        Modified?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler Modified;

    public ClockTemplate()
    {
        IEnumerable<Shape> shapes = CreateShapes();

        foreach (Shape shape in shapes)
            Items.Add(shape);
    }

    protected abstract IEnumerable<Shape> CreateShapes();

    protected override void InsertItem(int index, Shape item)
    {
        ArgumentNullException.ThrowIfNull(item);

        base.InsertItem(index, item);

        item.Changed += HandleShapeChanged;
    }

    protected override void RemoveItem(int index)
    {
        if (index >= 0 && index < Items.Count - 1)
        {
            Shape item = Items[index];
            item.Changed -= HandleShapeChanged;
        }

        base.RemoveItem(index);
    }

    protected override void SetItem(int index, Shape item)
    {
        if (index >= 0 && index < Items.Count - 1)
        {
            Shape oldItem = Items[index];
            oldItem.Changed -= HandleShapeChanged;
        }

        base.SetItem(index, item);

        item.Changed += HandleShapeChanged;
    }

    protected override void ClearItems()
    {
        foreach (Shape item in Items)
            item.Changed -= HandleShapeChanged;

        base.ClearItems();
    }

    private void HandleShapeChanged(object sender, EventArgs e)
    {
        IsNew = false;
    }
}