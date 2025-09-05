namespace WpfToolbox.Misc;

public static class Enumerable
{
    public static bool ContainsAll<TSource>(this IEnumerable<TSource> list, IEnumerable<TSource> cont)
    {
        return cont.All(x => list.Contains(x));
    }

    public static void AddRange<TSource>(this ObservableCollection<TSource> col, IEnumerable<TSource> add)
    {
        foreach (var item in add) col.Add(item);
    }

    public static void AddDistinguishRange<TSource>(this ObservableCollection<TSource> col, IEnumerable<TSource> add)
    {
        foreach (var item in add.Where(i => !col.Contains(i))) col.Add(item);
    }

    
    public static T? SingleOrNull<T>(this IList<T> list) where T : class
    {
        return list.Count == 1 ? list[0] : null;
    }

    //private static List<string> ToNames(this IEnumerable<DirectoryGroup>? values) => 
    //    values?.Select(g => g.Name).ToList() ?? []; 
}
