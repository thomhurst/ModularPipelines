using ModularPipelines.Logging;

namespace ModularPipelines.Azure.Pipelines;

public interface IAzurePipeline : IModuleOutputWriter
{
    public bool IsRunningOnAzurePipelines { get; }

    public AzurePipelineVariables Variables { get; }
}