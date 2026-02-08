using System.Collections.ObjectModel;

namespace DustInTheWind.ClockAvalonia.Demo.Utils;

internal static class EnumerableExtensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
    {
        return new ObservableCollection<T>(source);
    }
}
