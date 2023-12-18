using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "offerings", "show-terms")]
public record AzQuantumOfferingsShowTermsOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--provider-id")] string ProviderId,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
}