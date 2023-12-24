using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("snowball", "update-job")]
public record AwsSnowballUpdateJobOptions(
[property: CommandSwitch("--job-id")] string JobId
) : AwsOptions
{
    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--notification")]
    public string? Notification { get; set; }

    [CommandSwitch("--resources")]
    public string? Resources { get; set; }

    [CommandSwitch("--on-device-service-configuration")]
    public string? OnDeviceServiceConfiguration { get; set; }

    [CommandSwitch("--address-id")]
    public string? AddressId { get; set; }

    [CommandSwitch("--shipping-option")]
    public string? ShippingOption { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--snowball-capacity-preference")]
    public string? SnowballCapacityPreference { get; set; }

    [CommandSwitch("--forwarding-address-id")]
    public string? ForwardingAddressId { get; set; }

    [CommandSwitch("--pickup-details")]
    public string? PickupDetails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}