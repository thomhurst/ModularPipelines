using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "offerings", "accept-terms")]
public record AzQuantumOfferingsAcceptTermsOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--provider-id")] string ProviderId,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
}