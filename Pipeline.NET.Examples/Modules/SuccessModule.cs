using Pipeline.NET.Context;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.Examples.Modules;

public class SuccessModule : Module
{
    public SuccessModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
        return null;
    }
}