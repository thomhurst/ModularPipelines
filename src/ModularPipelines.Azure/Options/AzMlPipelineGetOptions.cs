using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "pipeline", "get")]
public record AzMlPipelineGetOptions(
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--pipeline-draft-id")]
    public string? PipelineDraftId { get; set; }

    [CliOption("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}