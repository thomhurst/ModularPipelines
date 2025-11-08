using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Build.Modules;

public class NugetVersionGeneratorModule : ModuleNew<string>, IModuleLifecycle
{
    private readonly IOptions<PublishSettings> _publishSettings;

    public NugetVersionGeneratorModule(IOptions<PublishSettings> publishSettings)
    {
        _publishSettings = publishSettings;
    }

    /// <inheritdoc/>
    public override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var gitVersionInformation = await context.Git().Versioning.GetGitVersioningInformation();

        var version = _publishSettings.Value.IsAlpha
            ? $"{gitVersionInformation.FullSemVer}-alpha{gitVersionInformation.CommitsSinceVersionSourcePadded!}"
            : gitVersionInformation.FullSemVer!;

        context.LogOnPipelineEnd($"Generated Version Number: {version}");

        return version;
    }

    /// <inheritdoc/>
    public Task OnBeforeExecuteAsync(IPipelineContext context)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task OnAfterExecuteAsync(IPipelineContext context, object? result, Exception? exception)
    {
        if (exception == null && result is string version)
        {
            context.Logger.LogInformation("NuGet Version to Package: {Version}", version);
        }
    }
}