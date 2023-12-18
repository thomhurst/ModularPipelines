using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "twin", "relationship", "show")]
public record AzDtTwinRelationshipShowOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--relationship-id")] string RelationshipId,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}