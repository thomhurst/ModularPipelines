using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "fhir-stores", "list")]
public record GcloudHealthcareFhirStoresListOptions(
[property: CliOption("--dataset")] string Dataset,
[property: CliOption("--location")] string Location
) : GcloudOptions;