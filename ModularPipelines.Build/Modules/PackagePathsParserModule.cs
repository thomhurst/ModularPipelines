using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[DependsOn<PackProjectsModule>]
public class PackagePathsParserModule : Module<List<string>>
{
    protected override async Task<ModuleResult<List<string>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packPackagesModuleResult = await GetModule<PackProjectsModule>();

        return packPackagesModuleResult.Value!
            .Select(x => x.StandardOutput)
            .Select(x => x.Split("Successfully created package '")[1])
            .Select(x => x.Split("'.")[0])
            .ToList();
    }
}