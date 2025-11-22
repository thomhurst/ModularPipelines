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
        where T : class, IModule
    {
        return modules.OfType<T>().Single();
    }

    /// <summary>
    /// Gets the first item from an IAsyncEnumerable.
    /// </summary>
    /// <param name="asyncEnumerable">The async enumerable.</param>
    /// <param name="cancellationToken">Used to cancel the operation.</param>
    /// <typeparam name="T">The type held within the enumerable.</typeparam>
    /// <returns>The first item if found.</returns>
    public static async Task<T?> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> asyncEnumerable, CancellationToken cancellationToken = default)
    {
        await foreach (var item in asyncEnumerable.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            return item;
        }

        return default;
    }

    internal static async IAsyncEnumerable<T> WhereAsync<T>(this IEnumerable<T> enumerable, Func<T, Task<bool>> condition)
    {
        foreach (var item in enumerable)
        {
            if (await condition(item))
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
}
