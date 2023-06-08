namespace ModularPipelines.Azure.Pipelines;

public class AzurePipeline<T> : IAzurePipeline<T>
{
    public AzurePipeline(AzurePipelineVariables variables)
    {
        Variables = variables;
    }

    public AzurePipelineVariables Variables { get; }
}