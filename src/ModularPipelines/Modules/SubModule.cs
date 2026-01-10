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

            var result = await action().ConfigureAwait(false);

            Duration = stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
            Status = Status.Successful;
            SubModuleResultTaskCompletionSource.TrySetResult(result);

            return result;
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            Duration = stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
            Status = Status.Failed;

            var wrappedException = new SubModuleFailedException(this, ex);
            SubModuleResultTaskCompletionSource.TrySetException(wrappedException);

            throw wrappedException;
        }
    }

    public override Task CallbackTask => SubModuleResultTaskCompletionSource.Task;
}