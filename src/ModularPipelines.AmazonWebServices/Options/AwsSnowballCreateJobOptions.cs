using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snowball", "create-job")]
public record AwsSnowballCreateJobOptions : AwsOptions
{
    [CliOption("--job-type")]
    public string? JobType { get; set; }

    [CliOption("--resources")]
    public string? Resources { get; set; }

    [CliOption("--on-device-service-configuration")]
    public string? OnDeviceServiceConfiguration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--address-id")]
    public string? AddressId { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--snowball-capacity-preference")]
    public string? SnowballCapacityPreference { get; set; }

    [CliOption("--shipping-option")]
    public string? ShippingOption { get; set; }

    [CliOption("--notification")]
    public string? Notification { get; set; }

    [CliOption("--cluster-id")]
    public string? ClusterId { get; set; }

    [CliOption("--snowball-type")]
    public string? SnowballType { get; set; }

    [CliOption("--forwarding-address-id")]
    public string? ForwardingAddressId { get; set; }

    [CliOption("--tax-documents")]
    public string? TaxDocuments { get; set; }

    [CliOption("--device-configuration")]
    public string? DeviceConfiguration { get; set; }

    [CliOption("--remote-management")]
    public string? RemoteManagement { get; set; }

    [CliOption("--long-term-pricing-id")]
    public string? LongTermPricingId { get; set; }

    [CliOption("--impact-level")]
    public string? ImpactLevel { get; set; }

    [CliOption("--pickup-details")]
    public string? PickupDetails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}