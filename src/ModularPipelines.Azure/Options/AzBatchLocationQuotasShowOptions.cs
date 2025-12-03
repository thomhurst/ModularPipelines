using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "location", "quotas", "show")]
public record AzBatchLocationQuotasShowOptions(
[property: CliOption("--location")] string Location
) : AzOptions;