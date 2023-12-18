using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "offerings", "accept-terms")]
public record AzQuantumOfferingsAcceptTermsOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--provider-id")] string ProviderId,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [BooleanCommandSwitch("--autoadd-only")]
    public bool? AutoaddOnly { get; set; }
}