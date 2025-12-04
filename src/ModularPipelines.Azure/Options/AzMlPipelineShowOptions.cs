using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "pipeline", "show")]
public record AzMlPipelineShowOptions(
[property: CliOption("--pipeline-id")] string PipelineId
) : AzOptions
{
    [CliOption("--output-file")]
    public string? OutputFile { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}