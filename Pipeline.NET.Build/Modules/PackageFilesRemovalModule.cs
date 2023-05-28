using Pipeline.NET.Attributes;
using Pipeline.NET.Context;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.Build.Modules;

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