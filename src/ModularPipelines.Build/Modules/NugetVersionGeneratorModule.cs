using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class NugetVersionGeneratorModule : IModule<string>
{
    private readonly IOptions<PublishSettings> _publishSettings;

    public NugetVersionGeneratorModule(IOptions<PublishSettings> publishSettings)
    {
        _publishSettings = publishSettings;
    }

    public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var gitVersionInformation = await context.Git().Versioning.GetGitVersioningInformation();

        var version = _publishSettings.Value.IsAlpha
            ? $"{gitVersionInformation.FullSemVer}-alpha{gitVersionInformation.CommitsSinceVersionSourcePadded!}"
            : gitVersionInformation.FullSemVer!;

        context.LogOnPipelineEnd($"Generated Version Number: {version}");

        return version;
    }
}