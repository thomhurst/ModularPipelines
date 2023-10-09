using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
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
        
        if (_publishSettings.Value.IsAlpha)
        {
            return $"{gitVersionInformation.FullSemVer}-alpha{gitVersionInformation.CommitsSinceVersionSourcePadded!}";
        }

        return gitVersionInformation.FullSemVer;
    }

    /// <inheritdoc/>
    protected override async Task OnAfterExecute(IPipelineContext context)
    {
        var moduleResult = await this;
        context.Logger.LogInformation("NuGet Version to Package: {Version}", moduleResult.Value);
    }
}
