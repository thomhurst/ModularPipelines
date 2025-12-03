using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quantum", "offerings", "show-terms")]
public record AzQuantumOfferingsShowTermsOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--provider-id")] string ProviderId,
[property: CliOption("--sku")] string Sku
) : AzOptions;