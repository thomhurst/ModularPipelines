using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snowball", "create-cluster")]
public record AwsSnowballCreateClusterOptions(
[property: CliOption("--job-type")] string JobType,
[property: CliOption("--address-id")] string AddressId,
[property: CliOption("--snowball-type")] string SnowballType,
[property: CliOption("--shipping-option")] string ShippingOption
) : AwsOptions
{
    [CliOption("--resources")]
    public string? Resources { get; set; }

    [CliOption("--on-device-service-configuration")]
    public string? OnDeviceServiceConfiguration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--notification")]
    public string? Notification { get; set; }

    [CliOption("--forwarding-address-id")]
    public string? ForwardingAddressId { get; set; }

    [CliOption("--tax-documents")]
    public string? TaxDocuments { get; set; }

    [CliOption("--remote-management")]
    public string? RemoteManagement { get; set; }

    [CliOption("--initial-cluster-size")]
    public int? InitialClusterSize { get; set; }

    [CliOption("--long-term-pricing-ids")]
    public string[]? LongTermPricingIds { get; set; }

    [CliOption("--snowball-capacity-preference")]
    public string? SnowballCapacityPreference { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}