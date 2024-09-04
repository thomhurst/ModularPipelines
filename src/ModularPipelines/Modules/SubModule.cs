using System.Diagnostics;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Modules;

internal class SubModule<T> : SubModuleBase
{
    internal Task<T> Task { get; }

    internal SubModule(Type parentModule, string name, Func<Task<T>> action) : base(parentModule, name)
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

        Task.ContinueWith(t =>
        {
            Duration = stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
            Status = t.IsCompletedSuccessfully ? Status.Successful : Status.Failed;
        });
    }

    public override Task CallbackTask => Task;
}