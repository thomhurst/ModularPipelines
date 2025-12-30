using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Attributes;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<RunUnitTestsModule>]
[DependsOn<PackagePathsParserModule>]
[RunOnLinuxOnly]
[SkipIfNoGitHubToken]
[RunOnlyOnBranch("main")]
[RunOnLinuxOnly]
public class UploadPackagesToNugetModule : Module<CommandResult[]>, ISkippable
{
    private readonly IOptions<NuGetSettings> _nugetSettings;
    private readonly IOptions<PublishSettings> _publishSettings;

    public UploadPackagesToNugetModule(IOptions<NuGetSettings> nugetSettings, IOptions<PublishSettings> publishSettings)
    {
        _nugetSettings = nugetSettings;
        _publishSettings = publishSettings;
    }

    public Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        return Task.FromResult<SkipDecision>(!_publishSettings.Value.ShouldPublish);
    }

    public override async Task<CommandResult[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_nugetSettings.Value.ApiKey);

        var packagePaths = context.GetModule<PackagePathsParserModule, List<File>>();

        return await NugetUploadHelper.UploadPackagesAsync(
            context,
            packagePaths.Value!,
            source: "https://api.nuget.org/v3/index.json",
            apiKey: _nugetSettings.Value.ApiKey,
            cancellationToken);
    }
}