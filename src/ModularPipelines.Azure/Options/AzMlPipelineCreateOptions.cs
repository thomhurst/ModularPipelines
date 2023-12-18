using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "pipeline", "create")]
public record AzMlPipelineCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pipeline-yaml")] string PipelineYaml
) : AzOptions
{
    [CommandSwitch("--continue")]
    public string? Continue { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}