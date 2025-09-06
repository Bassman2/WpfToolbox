namespace WpfToolbox.Misc;

/// <summary>
/// Provides extension methods for working with <see cref="IEnumerable{T}"/>, <see cref="ObservableCollection{T}"/>, and <see cref="IList{T}"/>.
/// Includes helpers for set containment, range addition, and safe single-item retrieval.
/// </summary>
public static class Enumerable
{
    /// <summary>
    /// Determines whether the source sequence contains all elements of the specified collection.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the collections.</typeparam>
    /// <param name="list">The source sequence to check.</param>
    /// <param name="cont">The collection of elements to look for.</param>
    /// <returns>True if all elements in <paramref name="cont"/> are contained in <paramref name="list"/>; otherwise, false.</returns>
    public static bool ContainsAll<TSource>(this IEnumerable<TSource> list, IEnumerable<TSource> cont)
    {
        return cont.All(x => list.Contains(x));
    }

    /// <summary>
    /// Adds a range of items to an <see cref="ObservableCollection{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the collection.</typeparam>
    /// <param name="col">The collection to add items to.</param>
    /// <param name="add">The items to add.</param>
    public static void AddRange<TSource>(this ObservableCollection<TSource> col, IEnumerable<TSource> add)
    {
        foreach (var item in add) col.Add(item);
    }

    /// <summary>
    /// Adds a range of items to an <see cref="ObservableCollection{T}"/>, skipping items that are already present.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the collection.</typeparam>
    /// <param name="col">The collection to add items to.</param>
    /// <param name="add">The items to add, only if not already present.</param>
    public static void AddDistinguishRange<TSource>(this ObservableCollection<TSource> col, IEnumerable<TSource> add)
    {
        foreach (var item in add.Where(i => !col.Contains(i))) col.Add(item);
    }


    /// <summary>
    /// Returns the single element in the list if it contains exactly one element; otherwise, returns null.
    /// </summary>
    /// <typeparam name="T">The reference type of the list elements.</typeparam>
    /// <param name="list">The list to check.</param>
    /// <returns>The single element if the list contains exactly one; otherwise, null.</returns>
    public static T? SingleOrNull<T>(this IList<T> list) where T : class
    {
        return list.Count == 1 ? list[0] : null;
    }

    //private static List<string> ToNames(this IEnumerable<DirectoryGroup>? values) => 
    //    values?.Select(g => g.Name).ToList() ?? []; 
}
