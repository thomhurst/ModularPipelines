using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<PackProjectsModule>]
[RunIfAll<ModularPipelines.Conditions.OnLinux>]
public class PackagePathsParserModule : Module<List<File>>
{
    private const string PackageCreationSuccessPrefix = "Successfully created package '";
    private const string PackagePathSuffix = "'.";

    protected override async Task<List<File>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packPackagesModuleResult = await context.GetModule<PackProjectsModule>();

        return packPackagesModuleResult.Value
            .Select(x => x.StandardOutput)
            .Select(x => x.Split(PackageCreationSuccessPrefix)[1])
            .Select(x => x.Split(PackagePathSuffix)[0])
            .Select(x => new File(x))
            .ToList();
    }
}
