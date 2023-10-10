using System.Diagnostics;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Modules;

public class SubModule : SubModuleBase
{
    internal Task Task { get; }

    internal SubModule(Type parentModule, string name, Func<Task> action) : base(parentModule, name)
    {
        StartTime = DateTimeOffset.UtcNow;
        var stopwatch = Stopwatch.StartNew();

        Task = Task.Run(async () =>
        {
            try
            {
                await action();
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
        });
    }
}

public class SubModule<T> : SubModule
{
    internal new Task<T> Task { get; }

    internal SubModule(Type parentModule, string name, Func<Task<T>> action) : base(parentModule, name, action)
    {
        StartTime = DateTimeOffset.UtcNow;
        var stopwatch = Stopwatch.StartNew();

        Task = System.Threading.Tasks.Task.Run(async () =>
        {
            try
            {
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
        });
    }
}
