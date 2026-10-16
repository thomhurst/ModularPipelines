using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Pipelines;

[ExcludeFromCodeCoverage]
public record AzurePipelineSystemVariables
{
    public AzurePipelinePullRequestVariables PullRequest { get; } = new();
}
