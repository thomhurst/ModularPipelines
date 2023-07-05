using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

[DependsOn<DependentOnSuccessModule>]
public class DependentOn2 : Module
{
    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync( IModuleContext context, CancellationToken cancellationToken )
    {
        await Task.Delay( TimeSpan.FromSeconds( 2 ), cancellationToken );
        return null;
    }
}
