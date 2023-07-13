namespace ModularPipelines.Azure.Pipelines;

internal class AzurePipeline : IAzurePipeline
{
    public AzurePipeline(AzurePipelineVariables variables)
    {
        Variables = variables;
    }

    public AzurePipelineVariables Variables { get; }
}
