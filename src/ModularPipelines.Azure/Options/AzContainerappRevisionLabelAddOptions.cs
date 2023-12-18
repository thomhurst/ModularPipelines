using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "revision", "label", "add")]
public record AzContainerappRevisionLabelAddOptions(
[property: CommandSwitch("--label")] string Label,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--revision")] string Revision
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-prompt")]
    public bool? NoPrompt { get; set; }
}