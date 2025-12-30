using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<AddLocalNugetSourceModule>]
[DependsOn<PackagePathsParserModule>]
[DependsOn<CreateLocalNugetFolderModule>]
[RunOnLinuxOnly]
public class UploadPackagesToLocalNuGetModule : Module<CommandResult[]>
{
    public override async Task<CommandResult[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var localRepoLocation = context.GetModule<CreateLocalNugetFolderModule, Folder>();
        var packagePaths = context.GetModule<PackagePathsParserModule, List<File>>();

        return await NugetUploadHelper.UploadPackagesAsync(
            context,
            packagePaths.Value!,
            source: localRepoLocation.Value.AssertExists()!.Path,
            apiKey: null,
            cancellationToken);
    }
}