namespace ModularPipelines.Azure.Pipelines;

public record AzurePipelineVariables
{
    public AzurePipelineAgentVariables Agent { get; }

    public AzurePipelineVariables(AzurePipelineAgentVariables agent)
    {
        Agent = agent;
    }
}
