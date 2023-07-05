namespace ModularPipelines.Extensions;

internal static class TaskExtensions
{
    public static async Task<TResult[]> WhenAllFailFast<TResult>(this ICollection<Task<TResult>> tasks)
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
}
