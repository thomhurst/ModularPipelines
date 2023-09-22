using ModularPipelines.Modules;

namespace ModularPipelines.Extensions;

public static class EnumerableExtensions
{
    public static T GetModule<T>(this IEnumerable<ModuleBase> modules) where T : ModuleBase => modules.OfType<T>().Single();

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
    
    public static async Task<T?> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> asyncEnumerable, CancellationToken cancellationToken = default)
    {
        await foreach (var item in asyncEnumerable.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            return item;
        }

        return default;
    }
}