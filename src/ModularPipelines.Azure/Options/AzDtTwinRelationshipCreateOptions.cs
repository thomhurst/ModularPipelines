using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "twin", "relationship", "create")]
public record AzDtTwinRelationshipCreateOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--kind")] string Kind,
[property: CliOption("--relationship-id")] string RelationshipId,
[property: CliOption("--source")] string Source,
[property: CliOption("--target")] string Target
) : AzOptions
{
    [CliFlag("--if-none-match")]
    public bool? IfNoneMatch { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}