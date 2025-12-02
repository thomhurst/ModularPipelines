using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<PackProjectsModule>]
[RunOnLinuxOnly]
public class PackagePathsParserModule : Module<List<File>>
{
    public override Task<List<File>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packPackagesModuleResult = context.GetModule<PackProjectsModule, CommandResult[]>();

        return Task.FromResult<List<File>?>(packPackagesModuleResult.Value!
            .Select(x => x.StandardOutput)
            .Select(x => x.Split("Successfully created package '")[1])
            .Select(x => x.Split("'.")[0])
            .Select(x => new File(x))
            .ToList());
    }
}