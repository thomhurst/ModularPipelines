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
        
        try
        {
            var result = await Task;

            Duration = stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
            Status = Status.Successful;
            
            return result;
        }
        catch
        {
            Duration = stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;
            Status = Status.Failed;
            throw;
        }
    }

    public override Task CallbackTask => Task;
}