using ModularPipelines.Context;

namespace ModularPipelines;

internal class BuildSystemDetector
{
    private readonly IEnvironmentContext _environment;

    public BuildSystemDetector(IEnvironmentContext environment)
    {
        _environment = environment;
    }
    
    public bool IsRunningOnAzurePipelines
        => !string.IsNullOrWhiteSpace(_environment.EnvironmentVariables.GetEnvironmentVariable("TF_BUILD"));
}