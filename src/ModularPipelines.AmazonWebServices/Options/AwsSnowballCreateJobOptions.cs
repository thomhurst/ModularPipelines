using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("snowball", "create-job")]
public record AwsSnowballCreateJobOptions : AwsOptions
{
    [CommandSwitch("--job-type")]
    public string? JobType { get; set; }

    [CommandSwitch("--resources")]
    public string? Resources { get; set; }

    [CommandSwitch("--on-device-service-configuration")]
    public string? OnDeviceServiceConfiguration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--address-id")]
    public string? AddressId { get; set; }

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--snowball-capacity-preference")]
    public string? SnowballCapacityPreference { get; set; }

    [CommandSwitch("--shipping-option")]
    public string? ShippingOption { get; set; }

    [CommandSwitch("--notification")]
    public string? Notification { get; set; }

    [CommandSwitch("--cluster-id")]
    public string? ClusterId { get; set; }

    [CommandSwitch("--snowball-type")]
    public string? SnowballType { get; set; }

    [CommandSwitch("--forwarding-address-id")]
    public string? ForwardingAddressId { get; set; }

    [CommandSwitch("--tax-documents")]
    public string? TaxDocuments { get; set; }

    [CommandSwitch("--device-configuration")]
    public string? DeviceConfiguration { get; set; }

    [CommandSwitch("--remote-management")]
    public string? RemoteManagement { get; set; }

    [CommandSwitch("--long-term-pricing-id")]
    public string? LongTermPricingId { get; set; }

    [CommandSwitch("--impact-level")]
    public string? ImpactLevel { get; set; }

    [CommandSwitch("--pickup-details")]
    public string? PickupDetails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}