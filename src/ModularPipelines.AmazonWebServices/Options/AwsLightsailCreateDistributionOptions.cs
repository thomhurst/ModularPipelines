using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-distribution")]
public record AwsLightsailCreateDistributionOptions(
[property: CommandSwitch("--distribution-name")] string DistributionName,
[property: CommandSwitch("--origin")] string Origin,
[property: CommandSwitch("--default-cache-behavior")] string DefaultCacheBehavior,
[property: CommandSwitch("--bundle-id")] string BundleId
) : AwsOptions
{
    [CommandSwitch("--cache-behavior-settings")]
    public string? CacheBehaviorSettings { get; set; }

    [CommandSwitch("--cache-behaviors")]
    public string[]? CacheBehaviors { get; set; }

    [CommandSwitch("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}