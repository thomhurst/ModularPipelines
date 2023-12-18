using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "twin", "relationship", "list")]
public record AzDtTwinRelationshipListOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [BooleanCommandSwitch("--incoming")]
    public bool? Incoming { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}