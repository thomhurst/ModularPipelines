using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.NuGet.Extensions;
using ModularPipelines.NuGet.Options;


namespace ModularPipelines.Build.Modules;

[DependsOn<RunUnitTestsModule>]
[DependsOn<PackagePathsParserModule>]
public class UploadPackagesToNugetModule : Module<List<CommandResult>>
{
    private readonly IOptions<NuGetSettings> _nugetSettings;
    private readonly IOptions<PublishSettings> _publishSettings;

    public UploadPackagesToNugetModule(IOptions<NuGetSettings> nugetSettings, IOptions<PublishSettings> publishSettings)
    {
        _nugetSettings = nugetSettings;
        _publishSettings = publishSettings;
    }

    protected override async Task OnBeforeExecute(IModuleContext context)
    {
        var packagePaths = await GetModule<PackagePathsParserModule>();

        foreach (var packagePath in packagePaths.Value!)
        {
            context.Logger.LogInformation("Uploading {File}", packagePath);
        }

        await base.OnBeforeExecute(context);
    }

    protected override async Task<bool> ShouldSkip(IModuleContext context)
    {
        var gitVersionInfo = await context.Git().Versioning.GetGitVersioningInformation();

        if (gitVersionInfo.BranchName != "main")
        {
            return true;
        }

        return !_publishSettings.Value.ShouldPublish;
    }

    protected override async Task<ModuleResult<List<CommandResult>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var gitVersionInformation = await context.Git().Versioning.GetGitVersioningInformation();

        if (gitVersionInformation.BranchName != "main")
        {
            return await NothingAsync();
        }
        
        ArgumentNullException.ThrowIfNull(_nugetSettings.Value.ApiKey);

        var packagePaths = await GetModule<PackagePathsParserModule>();

        return await context.NuGet()
            .UploadPackages(new NuGetUploadOptions(packagePaths.Value!.AsPaths(), new Uri("https://api.nuget.org/v3/index.json"))
            {
                ApiKey = _nugetSettings.Value.ApiKey!,
                NoSymbols = true
            });
    }
}
