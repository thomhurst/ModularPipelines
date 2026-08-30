using ModularPipelines.Reporting;
using ModularPipelines.Engine;

namespace ModularPipelines.Git;

internal sealed class GitRunReportEnricher(IGitInformation gitInformation) : IRunReportEnricher
{
    public async ValueTask EnrichAsync(
        RunReportEnrichmentContext context,
        CancellationToken cancellationToken)
    {
        var information = await gitInformation.GetInfoAsync(cancellationToken).ConfigureAwait(false);
        if (information is null)
        {
            return;
        }

        context.GitSha ??= information.LastCommitSha;
        context.GitBranch ??= information.BranchName;
    }
}
