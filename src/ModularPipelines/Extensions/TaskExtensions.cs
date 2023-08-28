using ModularPipelines.Models;

namespace ModularPipelines.Extensions;

public static class TaskExtensions
{
    internal static async Task<TResult[]> WhenAllFailFast<TResult>(this ICollection<Task<TResult>> tasks)
    {
        var originalTasks = tasks.ToList();

        tasks = tasks.ToList();

        while (tasks.Any())
        {
            var finished = await Task.WhenAny(tasks);

            // await to throw Exception if this Task errored
            await finished;

            tasks.Remove(finished);
        }

        return await Task.WhenAll(originalTasks);
    }

    public static Task<ModuleResult<T>?> AsTask<T>(this T t)
    {
        return Task.FromResult<ModuleResult<T>?>(new ModuleResult<T>(t));
    }
    
    public static Task<ModuleResult<TType>?> AsTask<TType, T>(this T t) where T : TType
    {
        return Task.FromResult<ModuleResult<TType>?>(new ModuleResult<TType>(t));
    }
}
