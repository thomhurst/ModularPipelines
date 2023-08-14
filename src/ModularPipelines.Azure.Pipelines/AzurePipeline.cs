using ModularPipelines.Context;

namespace ModularPipelines.Azure.Pipelines;

internal class AzurePipeline : IAzurePipeline
{
    private readonly IEnvironmentContext _environment;

    public AzurePipeline(AzurePipelineVariables variables, IEnvironmentContext environment)
    {
        _environment = environment;
        Variables = variables;
    }
    
    public bool IsRunningOnAzurePipelines
        => !string.IsNullOrWhiteSpace(_environment.EnvironmentVariables.GetEnvironmentVariable("TF_BUILD"));

    public AzurePipelineVariables Variables { get; }
}
