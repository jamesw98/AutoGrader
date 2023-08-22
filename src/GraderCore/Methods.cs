namespace GraderServices;

public static class Methods
{
    /// <summary>
    /// foreach with an counter
    /// https://stackoverflow.com/questions/521687/foreach-with-index
    /// </summary>
    /// <param name="ie"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
    {
        var i = 0;
        foreach (var e in ie) action(e, i++);
    }

    /// <summary>
    /// replaces:
    ///     / with \
    /// or:
    ///     \ with /
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string FixPathSeparators(this string? path)
    {
        const char tempSep = '@';
        return path?
            .Replace('\\', tempSep)
            .Replace('/', tempSep)
            .Replace(tempSep, Path.DirectorySeparatorChar) ?? "";
    }
}