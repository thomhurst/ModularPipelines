using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("edgeorder", "address", "create")]
public record AzEdgeorderAddressCreateOptions(
[property: CliOption("--address-name")] string AddressName,
[property: CliOption("--contact-details")] string ContactDetails,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--shipping-address")]
    public string? ShippingAddress { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}