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
    private const string PackageCreationSuccessPrefix = "Successfully created package '";
    private const string PackagePathSuffix = "'.";

    protected override Task<List<File>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packPackagesModuleResult = context.GetModule<PackProjectsModule, CommandResult[]>();

        return Task.FromResult<List<File>?>(packPackagesModuleResult.ValueOrDefault!
            .Select(x => x.StandardOutput)
            .Select(x => x.Split(PackageCreationSuccessPrefix)[1])
            .Select(x => x.Split(PackagePathSuffix)[0])
            .Select(x => new File(x))
            .ToList());
    }
}
