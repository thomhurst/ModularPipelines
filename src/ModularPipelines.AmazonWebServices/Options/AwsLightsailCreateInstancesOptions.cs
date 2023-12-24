using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-instances")]
public record AwsLightsailCreateInstancesOptions(
[property: CommandSwitch("--instance-names")] string[] InstanceNames,
[property: CommandSwitch("--availability-zone")] string AvailabilityZone,
[property: CommandSwitch("--blueprint-id")] string BlueprintId,
[property: CommandSwitch("--bundle-id")] string BundleId
) : AwsOptions
{
    [CommandSwitch("--custom-image-name")]
    public string? CustomImageName { get; set; }

    [CommandSwitch("--user-data")]
    public string? UserData { get; set; }

    [CommandSwitch("--key-pair-name")]
    public string? KeyPairName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--add-ons")]
    public string[]? AddOns { get; set; }

    [CommandSwitch("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}