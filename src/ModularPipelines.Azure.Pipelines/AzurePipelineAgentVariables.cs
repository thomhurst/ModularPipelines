using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Pipelines;

[ExcludeFromCodeCoverage]
public record AzurePipelineAgentVariables : AzurePipelineVariableBase
{
    public string BuildDirectory => Get("BuildDirectory")!;

    public string ContainerMapping => Get("ContainerMapping")!;

    public string HomeDirectory => Get("HomeDirectory")!;

    public string Id => Get("Id")!;

    public string JobName => Get("JobName")!;

    public string JobStatus => Get("JobStatus")!;

    public string MachineName => Get("MachineName")!;

    public string Name => Get("Name")!;

    public string OS => Get("OS")!;

    public string OSArchitecture => Get("OSArchitecture")!;

    public string TempDirectory => Get("TempDirectory")!;

    public string ToolsDirectory => Get("ToolsDirectory")!;

    public string WorkFolder => Get("WorkFolder")!;

    /// <inheritdoc/>
    protected override string Prefix => "Agent";
}