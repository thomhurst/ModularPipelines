using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Attributes;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[DependsOn<RunUnitTestsModule>]
[DependsOn<PackagePathsParserModule>]
[RunOnLinuxOnly]
[SkipIfDependabot]
[RunOnlyOnBranch("main")]
[RunOnLinuxOnly]
public class UploadPackagesToNugetModule : Module<CommandResult[]>
{
    private readonly IOptions<NuGetSettings> _nugetSettings;
    private readonly IOptions<PublishSettings> _publishSettings;

    public UploadPackagesToNugetModule(IOptions<NuGetSettings> nugetSettings, IOptions<PublishSettings> publishSettings)
    {
        _nugetSettings = nugetSettings;
        _publishSettings = publishSettings;
    }

    /// <inheritdoc/>
    protected override async Task OnBeforeExecute(IPipelineContext context)
    {
        var packagePaths = await GetModule<PackagePathsParserModule>();

        foreach (var packagePath in packagePaths.Value!)
        {
            context.Logger.LogInformation("Uploading {File}", packagePath);
        }

        await base.OnBeforeExecute(context);
    }

    /// <inheritdoc/>
    protected override Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        return Task.FromResult<SkipDecision>(!_publishSettings.Value.ShouldPublish);
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult[]?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_nugetSettings.Value.ApiKey);

        var packagePaths = await GetModule<PackagePathsParserModule>();

        return await packagePaths.Value!
            .SelectAsync(async nugetFile => await context.DotNet().Nuget.Push(new DotNetNugetPushOptions
            {
                Path = nugetFile,
                Source = "https://api.nuget.org/v3/index.json",
                ApiKey = _nugetSettings.Value.ApiKey!,
            }, cancellationToken), cancellationToken: cancellationToken)
            .ProcessOneAtATime();
    }
}