using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
[DependsOn<PackProjectsModule>]
[RunIf<ModularPipelines.OnLinux>]
public class PackagePathsParserModule : Module<List<FilePath>>
{
    private const string PackageCreationSuccessPrefix = "Successfully created package '";
    private const string PackagePathSuffix = "'.";

    protected override async Task<List<FilePath>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packPackagesModuleResult = await context.GetModule<PackProjectsModule>();

        List<FilePath> packages = [.. packPackagesModuleResult.Value
            .Select(x => x.StandardOutput)
            .Select(x => x.Split(PackageCreationSuccessPrefix)[1])
            .Select(x => x.Split(PackagePathSuffix)[0])
            .Select(x => new FilePath(x))];

        foreach (var package in packages)
        {
            PackedAssemblyVersionValidator.Validate(package.Path);
        }

        return packages;
    }
}
