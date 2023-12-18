using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "pipeline", "show")]
public record AzMlPipelineShowOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId
) : AzOptions
{
    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}