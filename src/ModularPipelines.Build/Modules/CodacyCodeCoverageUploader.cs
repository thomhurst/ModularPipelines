using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Build.Modules;

[DependsOn<MergeCoverageModule>]
[RunOnLinux]
[SkipIfNoGitHubToken]
[SkipIfNoStandardGitHubToken]
public class CodacyCodeCoverageUploader : Module<CommandResult>
{
    private readonly IOptions<CodacySettings> _options;

    public CodacyCodeCoverageUploader(IOptions<CodacySettings> options)
    {
        _options = options;
    }

    /// <inheritdoc/>
    protected override async Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        var mergeCoverageModuleResult = await GetModule<MergeCoverageModule>();

        if (mergeCoverageModuleResult.Value == null)
        {
            return "Merge coverage has no result";
        }

        if (string.IsNullOrEmpty(_options.Value.ApiKey))
        {
            return "No Codacy API key was found";
        }

        return false;
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var mergeCoverageModuleResult = await GetModule<MergeCoverageModule>();

        var coverageOutputFile = mergeCoverageModuleResult.Value ?? throw new FileNotFoundException("coverage.cobertura.xml");

        var scriptFile =
            await context.Downloader.DownloadFileAsync(
                new DownloadFileOptions(new Uri("https://coverage.codacy.com/get.sh")), cancellationToken);

        await context.Bash.Command(new BashCommandOptions($"ls -l {scriptFile}"), cancellationToken);

        await context.Bash.Command(new BashCommandOptions($"chmod u+x {scriptFile}"), cancellationToken);

        return await context.Bash.FromFile(new BashFileOptions(scriptFile)
        {
            Arguments = ["report", "-r", coverageOutputFile.Path],
            EnvironmentVariables = new Dictionary<string, string?>
            {
                ["CODACY_PROJECT_TOKEN"] = _options.Value.ApiKey,
            },
        }, cancellationToken);
    }
}