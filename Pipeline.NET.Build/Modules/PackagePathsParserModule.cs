using Pipeline.NET.Attributes;
using Pipeline.NET.Context;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.Build.Modules;

[DependsOn<PackProjectsModule>]
public class PackagePathsParserModule : Module<List<string>>
{
    public PackagePathsParserModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<List<string>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var packPackagesModuleResult = await GetModule<PackProjectsModule>();

        return packPackagesModuleResult.Value!
            .Select(x => x.Value.StandardOutput)
            .Select(x => x.Split("Successfully created package '")[1])
            .Select(x => x.Split("'.")[0])
            .ToList();
    }
}