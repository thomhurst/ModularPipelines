namespace ModularPipelines.Helpers;

internal static class TaskObservation
{
    public static void ObserveFault(Task task)
    {
        _ = task.ContinueWith(
            static faultedTask => _ = faultedTask.Exception,
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.Default);
    }
}
