using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Git.Attributes;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[PinToMaster]
[DependsOn<RunUnitTestsModule>]
[DependsOn<PackagePathsParserModule>]
[RunOnLinuxOnly]
[SkipIfNoGitHubToken]
[RunOnlyOnBranch("main")]
public class UploadPackagesToNugetModule : Module<CommandResult[]>
{
    private readonly IOptions<NuGetSettings> _nugetSettings;
    private readonly IOptions<PublishSettings> _publishSettings;

    public UploadPackagesToNugetModule(IOptions<NuGetSettings> nugetSettings, IOptions<PublishSettings> publishSettings)
    {
        _nugetSettings = nugetSettings;
        _publishSettings = publishSettings;
    }

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(() => !_publishSettings.Value.ShouldPublish)
        .Build();

    protected override async Task<CommandResult[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_nugetSettings.Value.ApiKey);

        var packagePaths = await context.GetModule<PackagePathsParserModule>();

        return await NugetUploadHelper.UploadPackagesAsync(
            context,
            packagePaths.ValueOrDefault!,
            source: _nugetSettings.Value.FeedUrl,
            apiKey: _nugetSettings.Value.ApiKey,
            cancellationToken);
    }
}