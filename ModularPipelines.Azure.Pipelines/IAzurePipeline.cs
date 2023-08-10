namespace ModularPipelines.Azure.Pipelines;

public interface IAzurePipeline
{
    public bool IsRunningOnAzurePipelines { get; }
    public AzurePipelineVariables Variables { get; }
}
