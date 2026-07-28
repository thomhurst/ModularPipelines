using System.Diagnostics;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Modules;

internal class SubModule<T> : SubModuleBase
{
    private Task _callbackTask = Task.CompletedTask;

    internal SubModule(Type parentModule, string name) : base(parentModule, name)
    {
    }

    public Task<T> Execute(Func<Task<T>> action)
    {
        var executionTask = ExecuteAsync(action);
        _callbackTask = executionTask;
        return executionTask;
    }

    public override Task CallbackTask => _callbackTask;

    private async Task<T> ExecuteAsync(Func<Task<T>> action)
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

            return result;
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            Duration = stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
            Status = Status.Failed;

            var wrappedException = new SubModuleFailedException(this, ex);
            throw wrappedException;
        }
    }
}
