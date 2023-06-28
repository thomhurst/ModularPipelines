using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class NugetVersionGeneratorModule : Module<string>
{
    protected override async Task<ModuleResult<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();
        
        return $"{GitVersionInformation.Major}.{GitVersionInformation.Minor}.{GitVersionInformation.Patch}-{GitVersionInformation.PreReleaseLabel}-{GitVersionInformation.CommitsSinceVersionSource}";
    }
}