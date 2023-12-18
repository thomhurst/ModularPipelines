using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "adaptive-application-controls", "list")]
public record AzSecurityAdaptiveApplicationControlsListOptions(
[property: CommandSwitch("--group-name")] string GroupName
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}