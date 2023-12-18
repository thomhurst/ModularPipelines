using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "pipeline", "get")]
public record AzMlPipelineGetOptions(
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [CommandSwitch("--pipeline-draft-id")]
    public string? PipelineDraftId { get; set; }

    [CommandSwitch("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}