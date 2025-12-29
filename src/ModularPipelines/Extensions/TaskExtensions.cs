namespace ModularPipelines.Extensions;

/// <summary>
/// Extensions for Tasks.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Turns a non-task into a task.
    /// </summary>
    /// <param name="t">Any object.</param>
    /// <typeparam name="T">The type of the object passed in.</typeparam>
    /// <returns>The object wrapped in a task.</returns>
    public static Task<T> AsTask<T>(this T t)
    {
        return Task.FromResult(t);
    }

    /// <summary>
    /// Turns a non-task into a task.
    /// </summary>
    /// <param name="t">Any object.</param>
    /// <typeparam name="TType">The type to cast the object to.</typeparam>
    /// <typeparam name="T">The type of the object passed in.</typeparam>
    /// <returns>The object wrapped in a task.</returns>
    public static Task<TType> AsTask<TType, T>(this T t)
        where T : TType
    {
        return Task.FromResult<TType>(t);
    }

    internal static async Task<TResult[]> WhenAllFailFast<TResult>(this ICollection<Task<TResult>> tasks)
    {
        var originalTasks = tasks.ToList();

        tasks = tasks.ToList();

        while (tasks.Any())
        {
            var finished = await Task.WhenAny(tasks).ConfigureAwait(false);

            // await to throw Exception if this Task errored
            await finished.ConfigureAwait(false);

            tasks.Remove(finished);
        }

        return await Task.WhenAll(originalTasks).ConfigureAwait(false);
    }
}