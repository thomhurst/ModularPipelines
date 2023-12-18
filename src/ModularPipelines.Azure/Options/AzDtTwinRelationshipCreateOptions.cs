using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "twin", "relationship", "create")]
public record AzDtTwinRelationshipCreateOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--kind")] string Kind,
[property: CommandSwitch("--relationship-id")] string RelationshipId,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--target")] string Target
) : AzOptions
{
    [BooleanCommandSwitch("--if-none-match")]
    public bool? IfNoneMatch { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}