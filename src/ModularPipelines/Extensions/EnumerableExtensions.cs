using ModularPipelines.Modules;

namespace ModularPipelines.Extensions;

/// <summary>
/// Extensions for enumerables.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Gets the specified module from the collection of modules.
    /// </summary>
    /// <param name="modules">The collection of modules.</param>
    /// <typeparam name="T">The type of module to get.</typeparam>
    /// <returns>The specified module.</returns>
    public static T GetModule<T>(this IEnumerable<IModule> modules)
        where T : IModule
    {
        return modules.OfType<T>().Single();
    }

    internal static async IAsyncEnumerable<T> WhereAsync<T>(this IEnumerable<T> enumerable, Func<T, Task<bool>> condition)
    {
        foreach (var item in enumerable)
        {
            if (await condition(item).ConfigureAwait(false))
            {
                yield return item;
            }
        }
    }

    internal static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> asyncEnumerable, CancellationToken cancellationToken = default)
    {
        var results = new List<T>();

        await foreach (var item in asyncEnumerable.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            results.Add(item);
        }

        return results;
    }

    internal static Dictionary<string, T> ToFirstByKeyDictionary<T>(
        this IEnumerable<T> source,
        Func<T, string> keySelector)
    {
        return source
            .GroupBy(keySelector, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.First(), StringComparer.Ordinal);
    }

    internal static Dictionary<string, T> ToUniqueByKeyDictionary<T>(
        this IEnumerable<T> source,
        Func<T, string> keySelector)
    {
        return source
            .GroupBy(keySelector, StringComparer.Ordinal)
            .Where(static group => group.Count() == 1)
            .ToDictionary(static group => group.Key, static group => group.First(), StringComparer.Ordinal);
    }
}
