using Pipeline.NET.Context;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.Examples.Modules;

public class FailedModule : Module
{
    public FailedModule(IModuleContext context) : base(context)
    {
    }

    public override bool IgnoreFailures => true;

    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

        throw new Exception();
    }
}