using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "twin", "relationship", "delete-all")]
public record AzDtTwinRelationshipDeleteAllOptions(
[property: CommandSwitch("--dt-name")] string DtName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}