namespace ModularPipelines.Azure.Pipelines;

public class AzurePipelineVariables
{
    public AzurePipelineAgentVariables Agent { get; }

    public AzurePipelineVariables(AzurePipelineAgentVariables agent)
    {
        Agent = agent;
    }
}
