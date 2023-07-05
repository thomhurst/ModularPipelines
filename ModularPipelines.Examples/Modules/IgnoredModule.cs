using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

[ModuleCategory( "Ignore" )]
public class IgnoredModule : Module
{
    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync( IModuleContext context, CancellationToken cancellationToken )
    {
        await Task.Delay( TimeSpan.FromSeconds( 15 ), cancellationToken );
        return null;
    }
}
