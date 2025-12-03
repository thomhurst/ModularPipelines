using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "twin", "relationship", "list")]
public record AzDtTwinRelationshipListOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliFlag("--incoming")]
    public bool? Incoming { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}