using System.Diagnostics;

namespace ModularPipelines.Modules;

public class SubModule : SubModuleBase
{
    internal Task Task { get; }

    internal SubModule(Type parentModule, string name, Func<Task> action) : base(parentModule, name)
    {
        StartTime = DateTimeOffset.UtcNow;
        var stopwatch = Stopwatch.StartNew();
        Task = action();
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
        Task = action();
        Task.ContinueWith(t =>
        {
            Duration = stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
        });
    }
}
