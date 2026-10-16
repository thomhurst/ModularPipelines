using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Pipelines;

[ExcludeFromCodeCoverage]
public record AzurePipelinePullRequestVariables : AzurePipelineVariableBase
{
    public string? IsFork => Get("IsFork");

    public string? PullRequestId => Get("PullRequestId");

    public string? PullRequestNumber => Get("PullRequestNumber");

    public string? SourceBranch => Get("SourceBranch");

    public string? SourceCommitId => Get("SourceCommitId");

    public string? SourceRepositoryUri => Get("SourceRepositoryUri");

    public string? TargetBranch => Get("TargetBranch");

    public string? TargetBranchName => Get("targetBranchName");

    /// <inheritdoc/>
    protected override string Prefix => "System_PullRequest";
}
