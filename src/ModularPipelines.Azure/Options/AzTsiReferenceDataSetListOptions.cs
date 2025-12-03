using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tsi", "reference-data-set", "list")]
public record AzTsiReferenceDataSetListOptions(
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;