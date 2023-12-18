using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "twin", "relationship", "update")]
public record AzDtTwinRelationshipUpdateOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--json-patch")] string JsonPatch,
[property: CommandSwitch("--relationship-id")] string RelationshipId,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}