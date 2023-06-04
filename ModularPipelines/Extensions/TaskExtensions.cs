namespace ModularPipelines.Extensions;

internal static class TaskExtensions
{
    public static async Task WhenAllFailFast(this ICollection<Task> tasks)
    {
        while (tasks.Any())
        {
            var finished = await Task.WhenAny(tasks);
                    
            // await to throw Exception if this Task errored
            await finished;
                    
            tasks.Remove(finished);
        }
    }
}