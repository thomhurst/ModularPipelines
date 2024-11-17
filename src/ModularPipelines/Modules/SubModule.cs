using System.Diagnostics;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Modules;

internal class SubModule<T> : SubModuleBase
{
    internal readonly TaskCompletionSource<T> SubModuleResultTaskCompletionSource = new();

    internal SubModule(Type parentModule, string name) : base(parentModule, name)
    {
    }

    public async Task<T> Execute(Func<Task<T>> action)
    {
        StartTime = DateTimeOffset.UtcNow;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            Status = Status.Processing;

            var result = await action();

            Duration = stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
            Status = Status.Successful;
            SubModuleResultTaskCompletionSource.SetResult(result);
        }
        catch (Exception ex)
        {
            Duration = stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
            Status = Status.Failed;
            SubModuleResultTaskCompletionSource.SetException(new SubModuleFailedException(this, ex));
        }

        return await SubModuleResultTaskCompletionSource.Task;
    }

    public override Task CallbackTask => SubModuleResultTaskCompletionSource.Task;
}