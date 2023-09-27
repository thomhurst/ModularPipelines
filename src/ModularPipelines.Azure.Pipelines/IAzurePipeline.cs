using ModularPipelines.Interfaces;

namespace ModularPipelines.Azure.Pipelines;

public interface IAzurePipeline : ICollapsableLogging
{
    public bool IsRunningOnAzurePipelines { get; }

    public AzurePipelineVariables Variables { get; }
}
