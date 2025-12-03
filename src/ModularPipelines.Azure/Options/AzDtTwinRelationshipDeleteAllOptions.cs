using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "twin", "relationship", "delete-all")]
public record AzDtTwinRelationshipDeleteAllOptions(
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}