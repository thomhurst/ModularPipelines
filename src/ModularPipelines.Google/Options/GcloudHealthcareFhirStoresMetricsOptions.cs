using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "fhir-stores", "metrics")]
public record GcloudHealthcareFhirStoresMetricsOptions(
[property: CliArgument] string FhirStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions;