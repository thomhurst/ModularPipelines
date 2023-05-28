using Pipeline.NET.Attributes;
using Pipeline.NET.Context;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.Build.Modules;

[DependsOn<CleanModule>]
public class BinObjFolderRemovalModule : Module
{
    public BinObjFolderRemovalModule(IModuleContext context) : base(context)
    {
    }

    protected override Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var binObjFolders = Context.FileSystem.GetFolders(Context.Environment.GitRootDirectory!.FullName,
            SearchOption.AllDirectories,
            path =>
                !path.FullName.Contains("Pipeline.NET.Build")
                && path.Name is "bin" or "obj");

        foreach (var binObjFolder in binObjFolders)
        {
            binObjFolder.Delete(true);
        }

        return NothingAsync();
    }
}