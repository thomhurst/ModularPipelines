using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "fhir-stores", "deidentify")]
public record GcloudHealthcareFhirStoresDeidentifyOptions(
[property: CliArgument] string FhirStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--destination-store")] string DestinationStore
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}