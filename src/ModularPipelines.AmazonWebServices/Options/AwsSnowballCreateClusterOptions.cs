using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("snowball", "create-cluster")]
public record AwsSnowballCreateClusterOptions(
[property: CommandSwitch("--job-type")] string JobType,
[property: CommandSwitch("--address-id")] string AddressId,
[property: CommandSwitch("--snowball-type")] string SnowballType,
[property: CommandSwitch("--shipping-option")] string ShippingOption
) : AwsOptions
{
    [CommandSwitch("--resources")]
    public string? Resources { get; set; }

    [CommandSwitch("--on-device-service-configuration")]
    public string? OnDeviceServiceConfiguration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--notification")]
    public string? Notification { get; set; }

    [CommandSwitch("--forwarding-address-id")]
    public string? ForwardingAddressId { get; set; }

    [CommandSwitch("--tax-documents")]
    public string? TaxDocuments { get; set; }

    [CommandSwitch("--remote-management")]
    public string? RemoteManagement { get; set; }

    [CommandSwitch("--initial-cluster-size")]
    public int? InitialClusterSize { get; set; }

    [CommandSwitch("--long-term-pricing-ids")]
    public string[]? LongTermPricingIds { get; set; }

    [CommandSwitch("--snowball-capacity-preference")]
    public string? SnowballCapacityPreference { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}