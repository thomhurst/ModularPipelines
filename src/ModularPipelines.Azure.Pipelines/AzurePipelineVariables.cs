using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Pipelines;

[ExcludeFromCodeCoverage]
public record AzurePipelineVariables
{
    public AzurePipelineAgentVariables Agent { get; }

    public AzurePipelineVariables(AzurePipelineAgentVariables agent)
    {
        Agent = agent;
    }
}