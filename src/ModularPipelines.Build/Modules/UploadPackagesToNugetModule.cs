using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Attributes;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
[DependsOn<RunAllUnitTestsModule>]
[DependsOn<PackagePathsParserModule>]
[RunIf<ModularPipelines.OnLinux>]
[SkipIfNoGitHubToken]
[RunOnlyOnBranch("main")]
public class UploadPackagesToNugetModule(IOptions<NuGetSettings> nugetSettings, IOptions<PublishSettings> publishSettings) : Module<CommandResult[]>
{
    private readonly IOptions<NuGetSettings> _nugetSettings = nugetSettings;
    private readonly IOptions<PublishSettings> _publishSettings = publishSettings;

    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithSkipWhen(_ => !_publishSettings.Value.ShouldPublish, "The 'ShouldPublish' flag is false");

    protected override async Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_nugetSettings.Value.ApiKey);

        var packagePaths = await context.GetModule<PackagePathsParserModule>();

        return await NugetUploadHelper.UploadPackagesAsync(
            context,
            packagePaths.Value,
            source: _nugetSettings.Value.FeedUrl,
            apiKey: _nugetSettings.Value.ApiKey,
            cancellationToken);
    }
}
