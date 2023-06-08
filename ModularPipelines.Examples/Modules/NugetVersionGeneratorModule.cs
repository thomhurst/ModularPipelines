using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class NugetVersionGeneratorModule : Module<string>
{
    protected override async Task<ModuleResult<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();
        
        var gitInformation = context.Git().Information;

        return $"{gitInformation.Major}.{gitInformation.Minor}.{gitInformation.Patch}-{gitInformation.Label}-{gitInformation.CommitsOnBranch:D2}";
    }
}