using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snowball", "update-job")]
public record AwsSnowballUpdateJobOptions(
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--notification")]
    public string? Notification { get; set; }

    [CliOption("--resources")]
    public string? Resources { get; set; }

    [CliOption("--on-device-service-configuration")]
    public string? OnDeviceServiceConfiguration { get; set; }

    [CliOption("--address-id")]
    public string? AddressId { get; set; }

    [CliOption("--shipping-option")]
    public string? ShippingOption { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--snowball-capacity-preference")]
    public string? SnowballCapacityPreference { get; set; }

    [CliOption("--forwarding-address-id")]
    public string? ForwardingAddressId { get; set; }

    [CliOption("--pickup-details")]
    public string? PickupDetails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}