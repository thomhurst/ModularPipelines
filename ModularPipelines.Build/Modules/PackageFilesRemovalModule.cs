using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[DependsOn<CleanModule>]
public class PackageFilesRemovalModule : Module
{
    public PackageFilesRemovalModule(IModuleContext context) : base(context)
    {
    }

    protected override Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var packageFiles = Context.FileSystem.GetFiles(Context.Environment.GitRootDirectory!.FullName,
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