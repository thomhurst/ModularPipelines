using ModularPipelines.Context;
using ModularPipelines.Logging;

namespace ModularPipelines.Azure.Pipelines;

internal class AzurePipeline : IAzurePipeline
{
    private readonly IEnvironmentContext _environment;
    private readonly IModuleOutputWriter _outputWriter;

    public AzurePipeline(AzurePipelineVariables variables,
        IEnvironmentContext environment,
        IModuleOutputWriterFactory outputWriterFactory)
    {
        _environment = environment;
        Variables = variables;
        _outputWriter = outputWriterFactory.Create("AzurePipeline");
    }

    public bool IsRunningOnAzurePipelines
        => !string.IsNullOrWhiteSpace(_environment.EnvironmentVariables.GetEnvironmentVariable("TF_BUILD"));

    public AzurePipelineVariables Variables { get; }

    public void WriteLine(string message)
    {
        _outputWriter.WriteLine(message);
    }

    public void WriteLineDirect(string message)
    {
        _outputWriter.WriteLineDirect(message);
    }

    public IDisposable BeginSection(string name)
    {
        return _outputWriter.BeginSection(name);
    }
}