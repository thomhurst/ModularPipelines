using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-instances")]
public record AwsLightsailCreateInstancesOptions(
[property: CliOption("--instance-names")] string[] InstanceNames,
[property: CliOption("--availability-zone")] string AvailabilityZone,
[property: CliOption("--blueprint-id")] string BlueprintId,
[property: CliOption("--bundle-id")] string BundleId
) : AwsOptions
{
    [CliOption("--custom-image-name")]
    public string? CustomImageName { get; set; }

    [CliOption("--user-data")]
    public string? UserData { get; set; }

    [CliOption("--key-pair-name")]
    public string? KeyPairName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--add-ons")]
    public string[]? AddOns { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}