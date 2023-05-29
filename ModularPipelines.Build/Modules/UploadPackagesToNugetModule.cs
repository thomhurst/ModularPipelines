using CliWrap.Buffered;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.NuGet.Extensions;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.Build.Modules;

[DependsOn<RunUnitTestsModule>]
[DependsOn<PackagePathsParserModule>]
public class UploadPackagesToNugetModule : Module<List<BufferedCommandResult>>
{
    private readonly IOptions<NuGetSettings> _options;

    public UploadPackagesToNugetModule(IModuleContext context, IOptions<NuGetSettings> options) : base(context)
    {
        ArgumentNullException.ThrowIfNull(options.Value.ApiKey);
        _options = options;
    }

    protected override async Task InitialiseAsync()
    {
        var packagePaths = await GetModule<PackagePathsParserModule>();
        
        foreach (var packagePath in packagePaths.Value!)
        {
            Context.Logger.LogInformation("Uploading {File}", packagePath);
        }
        
        await base.InitialiseAsync();
    }

    protected override async Task<ModuleResult<List<BufferedCommandResult>>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var packagePaths = await GetModule<PackagePathsParserModule>();

        return await Context.NuGet()
            .UploadPackage(new NuGetUploadOptions(packagePaths.Value!, new Uri("https://api.nuget.org/v3/index.json"))
            {
                ApiKey = _options.Value.ApiKey!
            });
    }
}