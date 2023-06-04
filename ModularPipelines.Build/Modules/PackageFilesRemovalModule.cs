using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[DependsOn<CleanModule>]
public class PackageFilesRemovalModule : Module
{
    protected override Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packageFiles = context.FileSystem.GetFiles(context.Environment.GitRootDirectory!.Path,
            SearchOption.AllDirectories,
            path =>
                path.Extension == ".nupkg");

        foreach (var packageFile in packageFiles)
        {
            packageFile.Delete();
        }

        return NothingAsync();
    }
}