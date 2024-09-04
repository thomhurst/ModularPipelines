using System.Diagnostics;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Modules;

internal class SubModule<T> : SubModuleBase
{
    internal Task<T> Task { get; private set; } = null!;

    internal SubModule(Type parentModule, string name) : base(parentModule, name)
    {
    }

    public async Task<T> Execute(Func<Task<T>> action)
    {
        StartTime = DateTimeOffset.UtcNow;
        var stopwatch = Stopwatch.StartNew();

        Task = System.Threading.Tasks.Task.Run(async () =>
        {
            try
            {
                Status = Status.Processing;
                return await action();
            }
            catch (Exception e)
            {
                throw new SubModuleFailedException(this, e);
            }
        });

        _ = Task.ContinueWith(t =>
        {
            Duration = stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
            Status = t.IsCompletedSuccessfully ? Status.Successful : Status.Failed;
        });

        return await Task;
    }

    public override Task CallbackTask => Task;
}