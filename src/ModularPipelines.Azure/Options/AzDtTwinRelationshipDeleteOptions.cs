using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "twin", "relationship", "delete")]
public record AzDtTwinRelationshipDeleteOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--relationship-id")] string RelationshipId,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}