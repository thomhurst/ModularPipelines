using Pipeline.NET.Attributes;
using Pipeline.NET.Context;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.Examples.Modules;

[ModuleCategory("Ignore")]
public class IgnoredModule : Module
{
    public IgnoredModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
        return null;
    }
}