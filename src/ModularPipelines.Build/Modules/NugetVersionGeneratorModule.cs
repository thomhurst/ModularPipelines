using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class NugetVersionGeneratorModule : Module<string>
{
    private readonly IOptions<PublishSettings> _publishSettings;

    public NugetVersionGeneratorModule(IOptions<PublishSettings> publishSettings)
    {
        _publishSettings = publishSettings;
    }

    /// <inheritdoc/>
    protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var gitVersionInformation = await context.Git().Versioning.GetGitVersioningInformation();

        var version = _publishSettings.Value.IsAlpha 
            ? $"{gitVersionInformation.FullSemVer}-alpha{gitVersionInformation.CommitsSinceVersionSourcePadded!}" 
            : gitVersionInformation.FullSemVer!;

        context.LogOnPipelineEnd($"Generated Version Number: {version}");

        return version;
    }

    /// <inheritdoc/>
    protected override async Task OnAfterExecute(IPipelineContext context)
    {
        var moduleResult = await this;
        context.Logger.LogInformation("NuGet Version to Package: {Version}", moduleResult.Value);
    }
}