using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lab", "artifact-source", "list")]
public record AzLabArtifactSourceListOptions(
[property: CommandSwitch("--lab-name")] string LabName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--orderby")]
    public string? Orderby { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}

