using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quantum", "offerings", "accept-terms")]
public record AzQuantumOfferingsAcceptTermsOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--provider-id")] string ProviderId,
[property: CliOption("--sku")] string Sku
) : AzOptions;