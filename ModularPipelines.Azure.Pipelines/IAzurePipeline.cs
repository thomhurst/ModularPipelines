namespace ModularPipelines.Azure.Pipelines;

public interface IAzurePipeline
{
    public AzurePipelineVariables Variables { get; }
}

public interface IAzurePipeline<T> : IAzurePipeline
{
}