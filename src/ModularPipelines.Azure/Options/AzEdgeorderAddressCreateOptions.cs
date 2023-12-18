using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edgeorder", "address", "create")]
public record AzEdgeorderAddressCreateOptions(
[property: CommandSwitch("--address-name")] string AddressName,
[property: CommandSwitch("--contact-details")] string ContactDetails,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--shipping-address")]
    public string? ShippingAddress { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}