namespace ModularPipelines.Azure.Pipelines;

public class AzurePipeline : IAzurePipeline
{
    public AzurePipeline( AzurePipelineVariables variables )
    {
        Variables = variables;
    }

    public AzurePipelineVariables Variables { get; }
}
