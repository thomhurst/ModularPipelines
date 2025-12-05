using ModularPipelines.Context;
using ModularPipelines.Logging;

namespace ModularPipelines.Azure.Pipelines;

internal class AzurePipeline : IAzurePipeline
{
    private readonly IEnvironmentContext _environment;
    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    public AzurePipeline(AzurePipelineVariables variables,
        IEnvironmentContext environment,
        IModuleLoggerProvider moduleLoggerProvider)
    {
        _environment = environment;
        _moduleLoggerProvider = moduleLoggerProvider;
        Variables = variables;
    }

    public bool IsRunningOnAzurePipelines
        => !string.IsNullOrWhiteSpace(_environment.EnvironmentVariables.GetEnvironmentVariable("TF_BUILD"));

    public AzurePipelineVariables Variables { get; }

    public void StartConsoleLogGroup(string name)
    {
        LogToConsole(BuildSystemValues.AzurePipelines.StartBlock(name));
    }

    public void EndConsoleLogGroup(string name)
    {
        LogToConsole(BuildSystemValues.AzurePipelines.EndBlock);
    }

    public void LogToConsole(string value)
    {
        ((IConsoleWriter) _moduleLoggerProvider.GetLogger()).LogToConsole(value);
    }
}